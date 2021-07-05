import { Formik, Form, ErrorMessage } from 'formik';
import { observer } from 'mobx-react-lite';
import React from 'react';
import { Button, Header } from 'semantic-ui-react';
import MyTextInput from '../../app/common/form/MyTextInput';
import { useStore } from '../../app/store/store';
import * as Yup from 'yup';
import ValidationErrors from '../errors/ValidationErrors';

export default observer(function RegisterForm() {
  const { userStore } = useStore();

  const validationScheme = Yup.object({
    displayName: Yup.string().required(),
    email: Yup.string().required().email(),
    userName: Yup.string().required(),
    password: Yup.string().required(),
  });

  return (
    <Formik
      initialValues={{
        displayName: '',
        userName: '',
        email: '',
        password: '',
        error: null,
      }}
      onSubmit={(values, { setErrors }) =>
        userStore.register(values).catch((error) => {
          setErrors(error);
          console.log(error);
        })
      }
      validationSchema={validationScheme}
    >
      {({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
        <Form
          className="ui form error"
          autoComplete="off"
          onSubmit={handleSubmit}
        >
          <Header
            as="h2"
            content="Sign Up to MeetUp"
            color="teal"
            textAlign="center"
          />
          <MyTextInput name="displayName" placeholder="Display Name" />
          <MyTextInput name="userName" placeholder="User Name" />
          <MyTextInput name="email" placeholder="Email" />
          <MyTextInput name="password" placeholder="Password" type="password" />

          <ErrorMessage
            name="error"
            render={() => <ValidationErrors errors={errors} />}
          />
          <pre>{JSON.stringify(errors)}</pre>

          <Button
            positive
            disabled={!isValid || !dirty}
            loading={isSubmitting}
            content="Sign Up"
            type="submit"
            fluid
          />
        </Form>
      )}
    </Formik>
  );
});
