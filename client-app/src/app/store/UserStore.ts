import { makeAutoObservable, runInAction } from 'mobx';
import { history } from '../..';
import agent from '../api/agent';
import { User, UserFormValues } from '../models/User';
import { store } from './store';

export default class UserStore {
  user: User | null = null;
  constructor() {
    makeAutoObservable(this);
  }

  get isLoggedIn() {
    return !!this.user;
  }

  login = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.login(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      history.push('/activities');
      store.modalStore.closeModal();
      console.log(user);
    } catch (error) {
      throw error;
    }
  };

  logout = () => {
    window.localStorage.removeItem('jwt');
    store.commonStore.setToken(null);
    runInAction(() => {
      this.user = null;
    });
    history.push('/');
  };

  getUser = async () => {
    try {
      const user = await agent.Account.current();
    } catch (error) {
      throw error;
    }
  };

  register = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.register(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      history.push('/activities');
      store.modalStore.closeModal();
      console.log(user);
    } catch (error) {
      throw error;
    }
  };
}
