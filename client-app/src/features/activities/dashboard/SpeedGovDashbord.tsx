import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Grid } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { useStore } from '../../../app/store/store';
import SpeedGovFilters from './SpeedGovFilter';
import SpeedGovList from './SpeedGovList';

function SpeedGovDashbord() {
  const { activityStore } = useStore();
  const { loadActivities, activityRegistry } = activityStore;

  useEffect(() => {
    if (activityRegistry.size <= 1) loadActivities();
  }, [activityRegistry, loadActivities]);

  if (activityStore.loadingInitial)
    return <LoadingComponent content="Loading App" />;
  return (
    <Grid stackable>
      <Grid.Column width="10">
        <SpeedGovList />
      </Grid.Column>
      <Grid.Column width="6">
        <SpeedGovFilters />
      </Grid.Column>
    </Grid>
  );
}

export default observer(SpeedGovDashbord);
