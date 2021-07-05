import React, { useState } from 'react';
import { Button, Header, Segment } from 'semantic-ui-react';
import axios from 'axios';
import ValidationErrors from '../../features/errors/ValidationErrors';

export default function TestErrors() {
  const [errors, setErrors] = useState([]);
  const baseUrl = 'http://localhost:5000/';
  axios.defaults.headers = {
    authorization:
      'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJlZHV1aCIsIm5iZiI6MTYyNTA2MTc5MCwiZXhwIjoxNjI1NjY2NTkwLCJpYXQiOjE2MjUwNjE3OTB9.Cyt5aStd-1bNjCzzPA8R1bIyr_SuuZAMhrFAwCUHdpSivV1PRMR87bJ9I2hM8-obLktNWcbDWmmeBBnDZ0nxiA',
  };
  function handleNotFound() {
    axios
      .get(baseUrl + 'buggy/not-found')
      .catch((err) => console.log(err.response));
  }

  function handleBadRequest() {
    axios
      .get(baseUrl + 'buggy/bad-request')
      .catch((err) => console.log(err.response));
  }

  function handleServerError() {
    axios
      .get(baseUrl + 'buggy/server-error')
      .catch((err) => console.log(err.response));
  }

  function handleUnauthorised() {
    axios
      .get(baseUrl + 'buggy/unauthorised')
      .catch((err) => console.log(err.response));
  }

  function handleBadGuid() {
    axios
      .get(baseUrl + 'activities/notaguid')
      .catch((err) => console.log(err.response));
  }

  function handleValidationError() {
    axios.post(baseUrl + 'activities', {}).catch((err) => setErrors(err));
  }

  return (
    <>
      <Header as="h1" content="Test Error component" />
      <Segment>
        <Button.Group widths="7">
          <Button onClick={handleNotFound} content="Not Found" basic primary />
          <Button
            onClick={handleBadRequest}
            content="Bad Request"
            basic
            primary
          />
          <Button
            onClick={handleValidationError}
            content="Validation Error"
            basic
            primary
          />
          <Button
            onClick={handleServerError}
            content="Server Error"
            basic
            primary
          />
          <Button
            onClick={handleUnauthorised}
            content="Unauthorised"
            basic
            primary
          />
          <Button onClick={handleBadGuid} content="Bad Guid" basic primary />
        </Button.Group>
      </Segment>
      {errors && <ValidationErrors errors={errors} />}
    </>
  );
}
