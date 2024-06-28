import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { AuthCallback } from './components/AuthCallback';

import { action as loginAction } from './pages/Login';
import { action as registerAction } from './pages/Register';
import { action as addClassAction } from './pages/AddClass';
import { action as addNoteAction } from './pages/AddNote';
import { action as addToDoAction } from './pages/AddToDo';
import { action as editClassAction } from './pages/EditClass';
import { action as editNoteAction } from './pages/EditNote';
import { action as editToDoAction } from './pages/EditToDo';

import { loader as allClassesLoader } from './pages/AllClasses';
import { loader as allTagsLoader } from './pages/AllNotes';
import { loader as editClassLoader } from './pages/EditClass';
import { loader as editNoteLoader } from './pages/EditNote';
import { loader as editToDoLoader } from './pages/EditToDo';
import { loader as dashboardLoader } from './pages/Dashboard';

import {
  HomeLayout,
  DashboardLayout,
  Landing,
  Register,
  Login,
  Error,
  AddClass,
  EditClass,
  AllClasses,
  AllNotes,
  EditNote,
  AddNote,
  Dashboard,
  AddToDo,
  EditToDo,
  AllToDos,
} from './pages';

const router = createBrowserRouter([
  {
    path: '/',
    element: <HomeLayout />,
    errorElement: <Error />,
    children: [
      {
        index: true,
        element: <Landing />,
      },
      {
        path: 'register',
        element: <Register />,
        action: registerAction,
      },
      {
        path: 'login',
        element: <Login />,
        action: loginAction,
      },
      {
        path: 'dashboard',
        element: <DashboardLayout />,
        children: [
          {
            path: '',
            element: <Dashboard />,
            loader: dashboardLoader,
          },
          {
            path: 'classes',
            children: [
              {
                path: 'add',
                element: <AddClass />,
                action: addClassAction,
              },
              {
                path: 'edit/:id',
                element: <EditClass />,
                loader: editClassLoader,
                action: editClassAction,
              },
              {
                path: '',
                element: <AllClasses />,
                loader: allClassesLoader,
              },
            ],
          },
          {
            path: 'notes',
            children: [
              {
                path: 'add',
                element: <AddNote />,
                action: addNoteAction,
              },
              {
                path: 'edit/:id',
                element: <EditNote />,
                loader: editNoteLoader,
                action: editNoteAction,
              },
              {
                path: '',
                element: <AllNotes />,
                loader: allTagsLoader,
              },
            ],
          },
          {
            path: 'todos',
            children: [
              {
                path: 'add',
                element: <AddToDo />,
                action: addToDoAction,
              },
              {
                path: 'edit/:id',
                element: <EditToDo />,
                loader: editToDoLoader,
                action: editToDoAction,
              },
              {
                path: '',
                element: <AllToDos />,
                loader: allTagsLoader,
              },
            ],
          },
          {
            path: 'auth-callback',
            element: <AuthCallback />,
          },
        ],
      },
    ],
  },
]);

const App = () => {
  return <RouterProvider router={router} />;
};
export default App;
