import { IContactInfo } from "./IContactInfo"

export interface IDoctor {
    id: string,
    fullName: string,
    userName: string,
    profileImage: string,
    specialization: string,
    location: string
    contactInfo: IContactInfo[]
}