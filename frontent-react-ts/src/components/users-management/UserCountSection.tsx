import UserCountCard from './UserCountCard';
import { IAuthUser, RolesEnum } from '../../types/auth.types';
import { FaUser, FaUserCog, FaUserShield, FaUserTie, FaGlobe, FaBriefcase } from 'react-icons/fa';

interface IProps {
  usersList: IAuthUser[];
}

const UserCountSection = ({ usersList }: IProps) => {
  let superAdmins = 0;
  let admins = 0;
  let regionalManagers = 0;
  let wealthManagers = 0;
  let relationshipManagers = 0;
  let appUsers = 0;

  usersList.forEach((item) => {
    if (item.roles.includes(RolesEnum.SuperAdmin)) {
      superAdmins++;
    } else if (item.roles.includes(RolesEnum.Admin)) {
      admins++;
    } else if (item.roles.includes(RolesEnum.RegionalManager)) {
      regionalManagers++;
    } else if (item.roles.includes(RolesEnum.WealthManager)) {
      wealthManagers++;
    } else if (item.roles.includes(RolesEnum.RelationshipManager)) {
      relationshipManagers++;
    } else if (item.roles.includes(RolesEnum.AppUser)) {
      appUsers++;
    }
  });

  const userCountData = [
    { count: superAdmins, role: RolesEnum.SuperAdmin, icon: FaUserCog, color: '#3b3549' },
    { count: admins, role: RolesEnum.Admin, icon: FaUserShield, color: '#9333EA' },
    { count: regionalManagers, role: RolesEnum.RegionalManager, icon: FaGlobe, color: '#0B96BC' },
    { count: wealthManagers, role: RolesEnum.WealthManager, icon: FaBriefcase, color: '#1D4ED8' },
    { count: relationshipManagers, role: RolesEnum.RelationshipManager, icon: FaUserTie, color: '#F59E0B' },
    { count: appUsers, role: RolesEnum.AppUser, icon: FaUser, color: '#FEC223' },
  ];

  return (
    <div className='grid grid-cols-1 lg:grid-cols-4 gap-x-4'>
      {userCountData.map((item, index) => (
        <UserCountCard key={index} count={item.count} role={item.role} icon={item.icon} color={item.color} />
      ))}
    </div>
  );
};

export default UserCountSection;
