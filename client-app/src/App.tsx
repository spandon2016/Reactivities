import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';
import { Header, List } from 'semantic-ui-react';

function App() {

  // container activities
  // function to set setActivities
  const [activities, setActivities] = useState([]);

  useEffect(() => {
    axios.get('http://localhost:5000/api/activities').then(response => {
      console.log(response);
      setActivities(response.data);
    }).then(console.log).catch(function(error) {
      console.dir(error);
      console.log('error.status', error.status)
      if(!error.status){
        // network error
        console.log('networ error')
      }

    })
  }, [])

  return (
    <div>
      <Header as='h2' icon='users' content='Reactivities'/>
        <List>
            {activities.map((activity: any) => (
            <List.Item key={activity.id}>
              {activity.title}
            </List.Item>))}
        </List>
  
    </div>
  );
}

export default App;
