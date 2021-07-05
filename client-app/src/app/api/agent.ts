import { store } from './../store/store';
import { Activity } from './../activity';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { string } from 'yup/lib/locale';
import { toast } from 'react-toastify';
import { history } from '../..';
import { config } from 'process';
import { request } from 'http';
import { User, UserFormValues } from '../models/User';

axios.defaults.baseURL = 'http://localhost:5000/';
axios.interceptors.request.use((config) => {
  const token = store.commonStore.token;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axios.interceptors.response.use(
  async (response) => {
    await sleep(1000);
    return response;
  },
  (error: AxiosError) => {
    const { data, status, config } = error.response!;
    switch (status) {
      case 400:
        if (typeof data === 'string') {
          toast.error(data);
        }
        if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
          history.push('/not-found');
        }
        if (data.errors) {
          const modalStateErrors = [];
          for (const key in data.errors) {
            modalStateErrors.push(data.errors[key]);
          }
          throw modalStateErrors.flat();
        }
        break;
      case 401:
        toast.error('unauthorized');
        break;
      case 404:
        history.push('/not-found');
        break;
      case 500:
        store.commonStore.setServerError(data);
        history.push('/server-error');
        break;
      default:
        break;
    }
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody),
};

const Activities = {
  list: () => requests.get<Activity[]>('/Activities'),
  details: (id: string) => requests.get<Activity>(`/Activities/${id}`),
  create: (activity: Activity) => requests.post('/Activities', activity),
  update: (activity: Activity) =>
    requests.put(`/Activities/${activity.id}`, activity),
  delete: (id: string) => requests.del(`/Activities/${id}`),
};

const Account = {
  current: () => requests.get<User>('/api/account'),
  login: (user: UserFormValues) =>
    requests.post<User>('/api/account/login', user),
  register: (user: UserFormValues) =>
    requests.post<User>('/api/account/register', user),
};

const agent = {
  Activities,
  Account,
};

export default agent;
