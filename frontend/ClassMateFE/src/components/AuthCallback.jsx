import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import customRequest from '../utils/customRequest';
import { toast } from 'react-toastify';

export const AuthCallback = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const handleAuthCallback = async () => {
      const urlParams = new URLSearchParams(window.location.search);
      const code = urlParams.get('code');
      const stateParam = urlParams.get('state');

      if (code && stateParam) {
        const { redirectRoute, resourceId, requestRoute } = JSON.parse(
          decodeURIComponent(stateParam)
        );

        try {
          await customRequest.get(`/sync/google-auth-response?code=${code}`);

          await customRequest.post(requestRoute, {
            resourceId,
            platform: 0,
          });

          toast.success('Successfully added to Google Calendar!');
          navigate(redirectRoute);
        } catch (authError) {
          //toast.error('Error during authentication callback.');
        }
      } else {
        toast.error('Something went wrong, please try again later.');
        navigate(redirectRoute);
      }
    };

    handleAuthCallback();
  }, []);

  return <></>;
};

export default AuthCallback;
