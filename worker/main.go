package main

import (
    "context"
    "fmt"
	"time"
	"encoding/json"
	"net/http"
	"github.com/gomodule/redigo/redis"
	"os"
)

type Message struct {
	JobId string
	StepId string
	TaskName string
}

func listenPubSubChannels(ctx context.Context, redisServerAddr string,
    onStart func() error,
    onMessage func(channel string, data []byte) error,
    channels ...string) error {
    // A ping is set to the server with this period to test for the health of
    // the connection and server.
    const healthCheckPeriod = time.Minute

    c, err := redis.Dial("tcp", redisServerAddr,
        // Read timeout on server should be greater than ping period.
        redis.DialReadTimeout(healthCheckPeriod+10*time.Second),
        redis.DialWriteTimeout(10*time.Second))
    if err != nil {
        return err
    }
    defer c.Close()

    psc := redis.PubSubConn{Conn: c}

    if err := psc.Subscribe(redis.Args{}.AddFlat(channels)...); err != nil {
        return err
    }

    done := make(chan error, 1)

    // Start a goroutine to receive notifications from the server.
    go func() {
        for {
            switch n := psc.Receive().(type) {
            case error:
                done <- n
                return
            case redis.Message:
                if err := onMessage(n.Channel, n.Data); err != nil {
                    done <- err
                    return
                }
            case redis.Subscription:
                switch n.Count {
                case len(channels):
                    // Notify application when all channels are subscribed.
                    if err := onStart(); err != nil {
                        done <- err
                        return
                    }
                case 0:
                    // Return from the goroutine when all channels are unsubscribed.
                    done <- nil
                    return
                }
            }
        }
    }()

    ticker := time.NewTicker(healthCheckPeriod)
    defer ticker.Stop()
loop:
    for err == nil {
        select {
        case <-ticker.C:
            if err = psc.Ping(""); err != nil {
                break loop
            }
        case <-ctx.Done():
            break loop
        case err := <-done:
            // Return error from the receive goroutine.
            return err
        }
    }

    // Signal the receiving goroutine to exit by unsubscribing from all channels.
    psc.Unsubscribe()

    // Wait for goroutine to complete.
    return <-done
}

func markStepComplete(jobId string, stepId int) {

}

func main() {
	fmt.Printf("Starting worker\n")

	redisServerAddr := "redis:6379"
	taskName := os.Getenv("TASKNAME")

    ctx, _ := context.WithCancel(context.Background())

    err := listenPubSubChannels(ctx,
        redisServerAddr,
        func() error {
            return nil
        },
        func(channel string, message []byte) error {
			var msg Message
			fmt.Printf("Message received: %s\n", message)
			err := json.Unmarshal(message, &msg)
			if err != nil {
				fmt.Println("error:", err)
			}
			fmt.Printf("%+v\n", msg)
			if msg.TaskName == taskName {
				fmt.Println("Matched task")

				// Emulate "work"
				time.Sleep(10 * time.Second)

				url := fmt.Sprintf("http://api:80/job/%v/steps/%v/complete", msg.JobId, msg.StepId)
				req, err := http.NewRequest("POST", url, nil)
				if err != nil {
					fmt.Println("error:", err)
				}
				_, err = http.DefaultClient.Do(req)
				if err != nil {
					fmt.Println("error:", err)
				}
			}
            return nil
        },
        "messages")

    if err != nil {
        fmt.Println(err)
        return
    }
}