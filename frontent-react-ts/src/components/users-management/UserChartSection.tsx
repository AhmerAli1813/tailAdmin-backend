import { IAuthUserList, RolesEnum } from '../../types/auth.types';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Tooltip, Legend } from 'chart.js';
import { Line } from 'react-chartjs-2';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Tooltip, Legend);

interface IProps {
  usersList: IAuthUserList[];
}

const UserChartSection = ({ usersList }: IProps) => {
  const chartLabels = [RolesEnum.SuperAdmin, RolesEnum.Admin, RolesEnum.RegionalManager, RolesEnum.AppUser ,RolesEnum.RelationshipManager,RolesEnum.WealthManager];
  const chartValues = [];

  const SuperAdminsCount = usersList.filter((q) => q.role.includes(RolesEnum.SuperAdmin)).length;
  chartValues.push(SuperAdminsCount);

  const adminsCount = usersList.filter((q) => q.role.includes(RolesEnum.Admin)).length;
  chartValues.push(adminsCount);
  
  const regionalManagersCount = usersList.filter((q) => q.role.includes(RolesEnum.RegionalManager)).length;
  chartValues.push(regionalManagersCount);
  
  const wealthManagersCount = usersList.filter((q) => q.role.includes(RolesEnum.WealthManager)).length;
  chartValues.push(wealthManagersCount);
  
  const relationshipManagersCount = usersList.filter((q) => q.role.includes(RolesEnum.RelationshipManager)).length;
  chartValues.push(relationshipManagersCount);
  
  const appUsersCount = usersList.filter((q) => q.role.includes(RolesEnum.AppUser)).length;
  chartValues.push(appUsersCount);
  
  const superAdminsCount = usersList.filter((q) => q.role.includes(RolesEnum.SuperAdmin)).length;
  chartValues.push(superAdminsCount);
  

  const chartOptions = {
    responsive: true,
    scales: {
      x: {
        grid: { display: false },
      },
      y: {
        ticks: { stepSize: 5 },
      },
    },
  };

  const chartData = {
    labels: chartLabels,
    datasets: [
      {
        label: 'count',
        data: chartValues,
        borderColor: '#754eb475',
        backgroundColor: '#754eb4',
        pointBorderColor: 'transparent',
        tension: 0.25,
      },
    ],
  };

  return (
    <div className='col-span-1 lg:col-span-3 bg-white p-2 rounded-md'>
      <h1 className='text-xl font-bold mb-2'>Users Chart</h1>
      <Line options={chartOptions} data={chartData} className='bg-white p-2 rounded-md' />
    </div>
  );
};

export default UserChartSection;
