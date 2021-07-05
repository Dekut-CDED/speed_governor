import React from 'react';
import { Message } from 'semantic-ui-react';

interface Props {
  errors: any;
}

function ValidationErrors({ errors }: Props) {
  return (
    <Message error>
      <Message.List>
        {errors.map((err: any, i: React.Key | null | undefined) => (
          <Message.Item key={i}>{err}</Message.Item>
        ))}
      </Message.List>
    </Message>
  );
}

export default ValidationErrors;
