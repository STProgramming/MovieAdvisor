import { RequestDto } from "./request-dto";

export interface SessionDto{
    sessionId: number;
    requests: RequestDto[];
    dateTimeCreation: Date;
}