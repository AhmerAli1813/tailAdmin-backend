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
  RolesEnum.RegionalManager,
  RolesEnum.WealthManager,
  RolesEnum.RelationshipManager,
  RolesEnum.AppUser,
];

export const managerAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
  RolesEnum.RegionalManager,
  RolesEnum.WealthManager,
];

export const adminAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
];

export const superAdminAccessRoles = [
  RolesEnum.SuperAdmin,
];

export const regionalManagerAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
  RolesEnum.RegionalManager,
];

export const wealthManagerAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
  RolesEnum.RegionalManager,
  RolesEnum.WealthManager,
];

export const relationshipManagerAccessRoles = [
  RolesEnum.SuperAdmin,
  RolesEnum.Admin,
  RolesEnum.RegionalManager,
  RolesEnum.WealthManager,
  RolesEnum.RelationshipManager,
];

export const appUserAccessRoles = [
  RolesEnum.Admin, // AppUser data is controlled by Admin
  RolesEnum.RegionalManager, // Admin assigns control to RegionalManager
  RolesEnum.WealthManager, // Admin assigns control to WealthManager
];


// We need to specify which Roles can be updated by Logged-in user
export const allowedRolesForUpdateArray = (loggedInUser?: IAuthUser): string[] => {
  return loggedInUser?.roles.includes(RolesEnum.SuperAdmin)
    ? [RolesEnum.Admin, RolesEnum.AppUser, RolesEnum.RegionalManager,RolesEnum.RelationshipManager,RolesEnum.WealthManager]
    : [RolesEnum.AppUser, RolesEnum.RegionalManager,RolesEnum.RelationshipManager,RolesEnum.WealthManager];
};

// we need to control that SuperAdmin cannot change SuperAdmin role
// Also, Admin cannot change SuperAdmin role and admin role
export const isAuthorizedForUpdateRole = (loggedInUserRole: string, selectedUserRole: string) => {
  let result = true;
  if (loggedInUserRole === RolesEnum.SuperAdmin && selectedUserRole === RolesEnum.SuperAdmin) {
    result = false;
  } else if (
    loggedInUserRole === RolesEnum.Admin &&
    (selectedUserRole === RolesEnum.SuperAdmin || selectedUserRole === RolesEnum.Admin || selectedUserRole ===RolesEnum.RegionalManager)
  ) {
    result = false;
  }

  return result;
};
