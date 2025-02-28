import { ISelectDropDownDto } from "./App.types";

export interface IAccountBasicDto {
  fullName: string;
  phoneNumber: string;
  userName: string;
  email: string;
  address?: string;
  designation?: string;
  dC_Code: string;
 
}

export interface IAccountDto extends IAccountBasicDto {
  regionHeadId?: string;
  lineManagerId?: string;
  regionId: number;

}

export interface ILoginDto {
  userName: string;
  password: string;
}

export interface IUpdateRoleDto {
  userName: string;
  newRole: string;
}

export interface IAuthUser extends IAccountBasicDto {
  id: string;
  regionHeadId?: string;
  lineManagerId?: string;
  regionId: number;
  createdAt: string;
  roles: string[];
}
export interface IAuthUserList extends IAccountBasicDto {
  id: string;
  region: number;
  createdAt: string;
  role: string;
  lockoutEnabled: boolean;
}

export interface ILoginResponseDto {
  newToken: string;
  userInfo: IAuthUser;
  responseCode?: number;
}

export interface IAuthContextState {
  isAuthenticated: boolean;
  isAuthLoading: boolean;
  user?: IAuthUser;
}

export enum IAuthContextActionTypes {
  INITIAL = "INITIAL",
  LOGIN = "LOGIN",
  LOGOUT = "LOGOUT",
}

export interface IAuthContextAction {
  type: IAuthContextActionTypes;
  payload?: IAuthUser;
}

export interface IAuthContext {
  isAuthenticated: boolean;
  isAuthLoading: boolean;
  user?: IAuthUser;
  login: (userName: string, password: string) => Promise<void>;
  logout: () => void;
}
export enum RolesEnum {
  SuperAdmin = "SuperAdmin",
  Admin = "Admin",
  RegionalManager = "RegionalManager",
  WealthManager = "WealthManager",
  RelationshipManager = "RelationshipManager",
  AppUser = "AppUser",
}

export interface IAuthUserFilter {
  email?: string | null;
  startDate?: Date | null;
  endDate?: Date | null;
  userRoles?: string[] | null;
}
export interface IAuthStatusDto {
  userName: string;
  lockoutEnabled: boolean;
}
export interface IAuthDefaultPassword {
  userName: string;
}

export interface UserNameListDto {
  regionalUser?: ISelectDropDownDto[];
  relationShipUser?: ISelectDropDownDto[];
  regions?: ISelectDropDownDto[];
}
