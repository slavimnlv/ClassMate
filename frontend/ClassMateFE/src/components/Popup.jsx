import { useState } from 'react';
import customRequest from '../utils/customRequest';
import { toast } from 'react-toastify';

const Popup = ({ onClose, id, route }) => {
  const [email, setEmail] = useState('');
  const [selectedRole, setSelectedRole] = useState(1);

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handleRoleChange = (e) => {
    setSelectedRole(e.target.value);
  };

  const handleSubmit = async () => {
    try {
      const formData = {
        Email: email,
        Role: parseInt(selectedRole, 10),
        ResourceID: id,
      };
      await customRequest.post(`/${route}/share`, formData);
      toast.success('Sharing successful.');
    } catch (error) {
      toast.error(error?.response?.data?.message);
    }
    onClose();
  };
  const roles = [
    { value: 1, label: 'Editor' },
    { value: 2, label: 'Viewer' },
  ];

  return (
    <div className='popup-overlay'>
      <div className='popup'>
        <div className='top-row-popup'>
          <h2>Share</h2>
          <button className='close-btn' onClick={onClose}>
            X
          </button>
        </div>
        <input
          type='email'
          placeholder='Enter email'
          value={email}
          onChange={handleEmailChange}
        />
        <select value={selectedRole} onChange={handleRoleChange}>
          {roles.map((role) => (
            <option key={role.value} value={role.value}>
              {role.label}
            </option>
          ))}
        </select>
        <button onClick={handleSubmit}>Share</button>
      </div>
    </div>
  );
};

export default Popup;
