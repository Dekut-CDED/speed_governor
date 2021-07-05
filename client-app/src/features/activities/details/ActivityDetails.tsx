import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { useParams } from 'react-router';
import { Grid, GridColumn } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { useStore } from '../../../app/store/store';
import ActivityDetailedChat from './ActivityDetailedChat';
import ActivityDetailsHeader from './ActivityDetailsHeader';
import ActivityDetailsInfo from './ActivityDetailsInfo';
import ActivityDetailsSidebar from './ActivityDetailsSidebar';

function ActivityDetails() {
  const { activityStore } = useStore();
  const { selectedActivity, loadActivity, loadingInitial } = activityStore;

  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    if (id) loadActivity(id);
  }, [id, loadActivity]);

  if (loadingInitial || !selectedActivity) return <LoadingComponent />;

  return (
    <Grid>
      <GridColumn width={10}>
        <ActivityDetailsHeader activity={selectedActivity} />
        <ActivityDetailsInfo activity={selectedActivity} />
        <ActivityDetailedChat />
      </GridColumn>
      <GridColumn width={6}>
        <ActivityDetailsSidebar />
      </GridColumn>
    </Grid>
  );
}

export default observer(ActivityDetails);
