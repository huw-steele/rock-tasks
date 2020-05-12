import React, { useState } from 'react';
import { Grid, TextField, Button, Select, MenuItem, FormHelperText } from '@material-ui/core';
import { Redirect } from 'react-router-dom';

type Job = {
    name: string,
    steps: JobStep[]
}

type JobStep = {
    name: string,
    task: string
}

type Response = {
    id: string
};

const executeJob = async (job: Job): Promise<string> => {
    const resp = await fetch('http://localhost:8000/job', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(job)
      });
    const responseBody: Response = await resp.json();
    return responseBody.id;
}

export default () => {
    const [jobName, setJobName] = useState('New Job');
    const [steps, setSteps] = useState<JobStep[]>([]);
    const [working, setWorking] = useState<boolean>(false);
    const [jobId, setJobId] = useState<string | null>(null);

    const handleRun = async () => {
        setWorking(true);
        const id = await executeJob({ name: jobName, steps});
        setWorking(false);
        setJobId(id);
    }

    if (jobId !== null) {
        return <Redirect to={`/${jobId}`} />
    }

    return (<Grid container spacing={2}>
        <Grid item xs={12}>
            <TextField id="jobname" label="Job Name" value={jobName} onChange={(ev: React.ChangeEvent<HTMLInputElement>) => { setJobName(ev.currentTarget.value) }} />  
        </Grid>
        <div>
            <Button disabled={working} style={{marginRight: '5px'}} variant="contained" color="primary" onClick={() => setSteps([...steps, { name: "Step " + (steps.length + 1), task: 'task1' }])} >Add Step</Button>
            <Button disabled={working} variant="contained" color="secondary" onClick={handleRun}>Run</Button>
        </div>
        {
            steps.map((step, i) => (
                <Grid item key={i} xs={12}>
                    <TextField 
                        label="Step Name"
                        value={step.name} 
                        onChange={(ev) => setSteps([ ...steps.map((s, j) => j === i ? { task: s.task, name:ev.currentTarget.value } : s) ])}
                    />
                    <Select
                        labelId="demo-simple-select-helper-label"
                        id="demo-simple-select-helper"
                        value={step.task}
                        onChange={(ev: any) => setSteps([ ...steps.map((s, j) => j === i ? { task: ev.target.value, name: s.name} : s) ])}
                        >
                        <MenuItem value={'task1'}>Task 1</MenuItem>
                        <MenuItem value={'task2'}>Task 2</MenuItem>
                    </Select>
                    <FormHelperText>Task name</FormHelperText>
                </Grid>
            ))
        }
    </Grid>);
};