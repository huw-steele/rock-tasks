import React from 'react';
import Container from '@material-ui/core/Container';
import Typography from '@material-ui/core/Typography';
import Box from '@material-ui/core/Box';
import JobMaker from './JobMaker';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link
} from "react-router-dom";
import JobStatus from './JobStatus';

function App() {
  return (
    <Router>
      <Container maxWidth="sm">
        <Box my={4}>
          <Typography variant="h4" component="h1" gutterBottom>
            ForgeRock Task Runner
          </Typography>
          <Switch>
            <Route path="/:id">
              <JobStatus />
            </Route>
            <Route path="/">
              <JobMaker />
            </Route>
          </Switch>
        </Box>
      </Container>
    </Router>
  );
}

export default App;
