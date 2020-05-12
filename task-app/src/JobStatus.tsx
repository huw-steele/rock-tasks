import React, { useState, useEffect } from 'react';
import { Grid } from '@material-ui/core';
import { useParams } from 'react-router-dom';

type Job = {
    id: string,
    name: string,
    startedOn: Date | null,
    endedOn: Date | null,
    steps: JobStep[]
};

type JobStep = {
    id: string,
    name: string,
    task: string,
    startedOn: Date | null,
    endedOn: Date | null
};

const getJob = async (id: string, setter: (job: Job) => void) : Promise<void> => {
    const resp = await fetch(`http://localhost:8000/job/${id}`, {
        method: 'GET'
      });
    const job = await resp.json();
    setter(job);
}

export default () => {
    let { id } = useParams();
    let [job, setJob] = useState<Job | null>(null);
    useEffect(() => {
        getJob(id, setJob);
    }, []);

    return (
        <Grid container spacing={2}>
            { job === null && <span>Loading...</span> }
            { job !== null && <><Grid item xs={12}><h3>{ job.name }</h3></Grid>
                    { job?.steps.map(step => { return (
                            <Grid item xs={12}>
                                <div><h4>{ step.name } - {step.task}</h4></div>
                                <div>{ step.startedOn !== null ? `Started on ${step.startedOn}` : 'Not started' }</div>
                                <div>{ step.endedOn !== null ? `Ended on ${step.endedOn}` : 'Not ended' }</div>
                            </Grid>
                        )})
                    }
                </>
            }
        </Grid>
    );
}