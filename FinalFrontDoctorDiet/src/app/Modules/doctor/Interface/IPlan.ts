import { IAllergics } from "./IAllergics"
import { IDay } from "./IDay"

export interface IPlan {
    id?: number
    doctorId: string
    Duration: number | null
    CaloriesTo: number
    CaloriesFrom: number
    goal: any
    gender: any
    Days?: any[]
    Allergics?: IAllergics[]

}