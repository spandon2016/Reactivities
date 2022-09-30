import { makeAutoObservable  } from "mobx";
import { hasListeners } from "mobx/dist/internal";
import { ServerError} from "../models/serverError";

export default class CommonStore {
    error: ServerError | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    setServerError = (error: ServerError) => {
        this.error = error;
    }
}