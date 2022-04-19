import {IValidatorUser} from './IValidatorUser';

export default class ValidatorUser implements IValidatorUser{
  accountId: string;
  naam: string;
  id: number;
  account: any;
  bedrijfId: number;
  email?: string;
  wachtwoord?: string;
}
