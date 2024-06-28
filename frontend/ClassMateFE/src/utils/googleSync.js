import { toast } from 'react-toastify';
import customRequest from './customRequest';

export const handleCalendarSync = async (
  requestRoute,
  requestBody,
  redirectRoute
) => {
  try {
    await customRequest.post(requestRoute, requestBody);
    toast.success('Successfully added to Google Calendar');
  } catch (error) {
    if (
      error.response &&
      error.response.status === 404 &&
      error.response.data.message === 'No existing token found.'
    ) {
      try {
        const authUrlResponse = await customRequest.get(
          '/sync/google-auth-url'
        );
        const authUrl = authUrlResponse.data;

        const resourceId = requestBody.resourceId;

        const stateParam = encodeURIComponent(
          JSON.stringify({ redirectRoute, resourceId, requestRoute })
        );

        const redirectUrl = `${authUrl}&state=${stateParam}`;

        window.location.href = redirectUrl;
      } catch (authError) {
        toast.error('An authentication error occurred. Plese try again later!');
      }
    } else {
      toast.error('An error occurred. Plese try again later!');
    }
  }
};
