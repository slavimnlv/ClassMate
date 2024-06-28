import { Link, useRouteError } from 'react-router-dom';

const Error = () => {
  const error = useRouteError();
  console.log(error);
  return (
    <div className='error-page'>
      <h1>Something went wrong...</h1>
      <Link to='/'>
        <button>back home</button>
      </Link>
    </div>
  );
};
export default Error;
