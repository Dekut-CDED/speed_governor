import { format } from 'date-fns';
import React from 'react';
import { Link } from 'react-router-dom';
import { Button, Icon, Item, Segment } from 'semantic-ui-react';
import { Activity } from '../../../app/activity';

interface Props {
  activity: Activity;
}
function ActivityListItem({ activity }: Props) {
  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Image
              size="tiny"
              circular
              src="/assets/user.png"
            ></Item.Image>
            <Item.Content>
              <Item.Header as={Link} to={`/activities/${activity.id}`}>
                {activity.title}
              </Item.Header>
              <Item.Description>Hosted by Body</Item.Description>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment>
        <span>
          <Icon name="clock" /> {format(activity.date!, 'dd MMM yyy h:mmm aa')}
          <Icon name="marker" /> {activity.venue}
        </span>
      </Segment>
      <Segment secondary clearing>
        <span>Attendee goes here</span>
        <Button
          as={Link}
          to={`/activities/${activity.id}`}
          color="teal"
          floated="right"
          content="View"
        />
      </Segment>
    </Segment.Group>
  );
}

export default ActivityListItem;
