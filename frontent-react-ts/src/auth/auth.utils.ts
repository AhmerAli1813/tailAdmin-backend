import { IAuthUser, RolesEnum } from '../types/auth.types';
import axiosInstance from '../utils/axiosInstance';

export const setSession = (accessToken: string | null) => {
  if (accessToken) {
    localStorage.setItem('accessToken', accessToken);
    axiosInstance.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
  } else {
    localStorage.removeItem('accessToken');
    delete axiosInstance.defaults.headers.common.Authorization;
  }
};

export const getSession = () => {
  return localStorage.getItem('accessToken');
};

export const allAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
  RolesEnum.Investor,
  RolesEnum.Salesman,
  RolesEnum.AppUser,
];

export const InvestorAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
  RolesEnum.Investor
]

export const SalesmanAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
  RolesEnum.Salesman
]

export const adminAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
];

export const superAdminAccessRoles = [
  RolesEnum.SuperAdmin,
];







// We need to specify which Roles can be updated by Logged-in user
export const allowedRolesForUpdateArray = (loggedInUser?: IAuthUser): string[] => {
  return loggedInUser?.roles.includes(RolesEnum.SuperAdmin)
    ? [RolesEnum.Admin, RolesEnum.AppUser, RolesEnum.Investor,RolesEnum.Salesman]
    : [RolesEnum.AppUser, RolesEnum.Investor,RolesEnum.Salesman];
};

// we need to control that SuperAdmin cannot change SuperAdmin role
// Also, Admin cannot change SuperAdmin role and admin role
export const isAuthorizedForUpdateRole = (loggedInUserRole: string, selectedUserRole: string) => {
  let result = true;
  if (loggedInUserRole === RolesEnum.SuperAdmin && selectedUserRole === RolesEnum.SuperAdmin) {
    result = false;
  } else if (
    loggedInUserRole === RolesEnum.Admin &&
    (selectedUserRole === RolesEnum.SuperAdmin || selectedUserRole === RolesEnum.Admin)
  ) {
    result = false;
  }

  return result;
};
