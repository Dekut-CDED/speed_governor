import React, { useEffect } from 'react';
import { Container } from 'semantic-ui-react';
import './App.css';
import '../src/css/index.css';
import NavBar from './app/layout/NavBar';
import SpeedGovDashbord from './features/activities/dashboard/SpeedGovDashbord';
import { observer } from 'mobx-react-lite';
import { Route, Switch, useLocation } from 'react-router';
import HomePage from './app/layout/HomePage';
import SpeedGovForm from './features/activities/form/SpeedGovForm';
import SpeedGovDetailPage from './features/activities/details/SpeedGovDetails';
import TestErrors from './app/models/TestError';
import { ToastContainer } from 'react-toastify';
import NotFound from './features/errors/NotFound';
import ServerErrors from './features/errors/ServerErrors';
import LoginForm from './features/users/LoginForm';
import { useStore } from './app/store/store';
import LoadingComponent from './app/layout/LoadingComponent';
import ModalContainer from './app/common/modals/ModalContainer';

function App() {
  const location = useLocation();
  const { commonStore, userStore } = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  if (!commonStore.appLoaded)
    return <LoadingComponent content="loading app..." />;

  return (
    <>
      <ToastContainer position="bottom-right" hideProgressBar />
      <ModalContainer />
      <Route exact path="/" component={HomePage} />
      <Route
        path={'/(.+)'}
        render={() => (
          <>
            <NavBar />
            <Container style={{ marginTop: '7em' }}>
              <Switch>
                <Route exact path="/activities" component={SpeedGovDashbord} />
                <Route path="/activities/:id" component={SpeedGovDetailPage} />
                <Route
                  key={location.key}
                  path={['/createActivity', '/manage/:id']}
                  component={SpeedGovForm}
                />
                <Route path="/errors" component={TestErrors} />
                <Route path="/server-error" component={ServerErrors} />
                <Route path="/login" component={LoginForm} />
                <Route component={NotFound} />
              </Switch>
            </Container>
          </>
        )}
      />
    </>
  );
}

export default observer(App);
