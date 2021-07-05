import { makeAutoObservable } from 'mobx';
import { ChatComent } from '../models/ChatComent';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { store } from './store';

export default class CommentStore {
  comment: ChatComent[] = [];
  hubConnction: HubConnection | null = null;

  constructor() {
    makeAutoObservable(this);
  }

  //   createHubConnnection = (activityId: string) => {
  //     if (store.activityStore.selectedActivity) {
  //       this.hubConnction = new HubConnectionBuilder().withUrl(
  //         'http://localhost:5000/chat?activityId=' + activityId,
  //         {
  //         }
  //       );
  //     }
  //   };
}
