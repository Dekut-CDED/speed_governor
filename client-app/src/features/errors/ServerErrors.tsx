import { observer } from 'mobx-react-lite';
import React from 'react';
import { Container, Header, Segment } from 'semantic-ui-react';
import { useStore } from '../../app/store/store';

export default observer(function ServerErrors() {
  const { commonStore } = useStore();

  return (
    <Container>
      <Header as="h1" content="Server Error">
        <Header sub as="h5" color="red" content={commonStore.error?.message}>
          {commonStore.error?.details && (
            <Segment>
              <Header as="h4" content="Stack trace" color="teal"></Header>
              <code style={{ marginTop: '10px' }}>
                {commonStore.error.details}
              </code>
            </Segment>
          )}
        </Header>
      </Header>
    </Container>
  );
});
