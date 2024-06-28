import { useLoaderData } from 'react-router-dom';
import customRequest from '../utils/customRequest';
import { toast } from 'react-toastify';

export const loader = async () => {
  try {
    const [classData, todoData] = await Promise.all([
      customRequest.get('/classes/next'),
      customRequest.get('/todos/next'),
    ]);
    return { classData: classData.data, todoData: todoData.data };
  } catch (error) {
    toast.error(
      error?.response?.data?.message || 'An error occurred while fetching data'
    );
    return { classData: null, todoData: null };
  }
};

const Dashboard = () => {
  const getGreeting = () => {
    const currentHour = new Date().getHours();
    if (currentHour < 12) {
      return 'Good Morning!';
    } else if (currentHour < 18) {
      return 'Good Afternoon!';
    } else {
      return 'Good Evening!';
    }
  };

  const getDayName = (date) => {
    return date.toLocaleDateString(undefined, { weekday: 'long' });
  };

  const formatDate = (dateString) => {
    const dateObj = new Date(dateString);
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(today.getDate() + 1);

    if (dateObj.toDateString() === today.toDateString()) {
      return 'today';
    } else if (dateObj.toDateString() === tomorrow.toDateString()) {
      return 'tomorrow';
    } else if (
      dateObj < today ||
      (dateObj - today) / (1000 * 60 * 60 * 24) > 7
    ) {
      return `${dateObj.toLocaleDateString()} (${getDayName(dateObj)})`;
    } else {
      return getDayName(dateObj);
    }
  };

  const formatTime = (datetime) => {
    const date = new Date(datetime);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  const data = useLoaderData();

  return (
    <div className='dashboard-container'>
      <h1 className='greeting'>{getGreeting()}</h1>

      {data.classData ? (
        <div className='next-info'>
          <p className='next-info-title'>Your Next Class:</p>
          <div className='card'>
            <div className='card-content'>
              <p className='subtitle'>
                {formatDate(data.classData.startDate)},{' '}
                {formatTime(data.classData.startDate)}-
                {formatTime(data.classData.endDate)}
              </p>
              <p className='title'>{data.classData.title}</p>
              <p className='description'>{data.classData.description}</p>
            </div>
          </div>
        </div>
      ) : (
        <p className='no-data'>No Upcoming Classes Found.</p>
      )}

      {data.todoData ? (
        <div className='next-info'>
          <p className='next-info-title'>Your Next Due ToDo:</p>
          <div className='card'>
            <div className='card-content'>
              <p className='subtitle'>
                {formatDate(data.todoData.deadline)},{' '}
                {formatTime(data.todoData.deadline)}
              </p>
              <p className='title'>{data.todoData.title}</p>
              <p className='description'>{data.todoData.description}</p>
            </div>
          </div>
        </div>
      ) : (
        <p className='no-data'>No Upcoming ToDos Found.</p>
      )}
    </div>
  );
};

export default Dashboard;
