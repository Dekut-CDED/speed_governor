import { Formik, Form } from 'formik';
import { observer } from 'mobx-react-lite';
import React from 'react';
import { Button, Header } from 'semantic-ui-react';
import MyTextInput from '../../app/common/form/MyTextInput';
import { useStore } from '../../app/store/store';

export default observer(function LoginForm() {
  const { userStore } = useStore();

  return (
    <Formik
      initialValues={{ email: '', password: '' }}
      onSubmit={(values, { setErrors }) =>
        userStore.login(values).catch((error) =>
          setErrors({
            email: 'Invalid email or password',
          })
        )
      }
    >
      {({ handleSubmit, isSubmitting }) => (
        <Form className="ui form " autoComplete="off">
          <Header
            as="h2"
            content="Login to the Meetup"
            color="teal"
            textAlign="center"
          />
          <MyTextInput name="email" placeholder="Email" />
          <MyTextInput name="password" placeholder="Password" type="password" />

          <Button
            positive
            loading={isSubmitting}
            content="Login"
            type="submit"
            fluid
          />
        </Form>
      )}
    </Formik>
  );
});
