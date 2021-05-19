import React from 'react';
import { useFormik } from 'formik';
import axiosInstance from '../../Common/API';

const SignUp = () => {
    const formik = useFormik({
        initialValues: {
            userName: '',
            email: '',
            firstName: '',
            lastName: '',
            phoneNumber: '',
            password: '',
            passwordConfirm: '',
            dateOfBirth: ''
        },
        onSubmit: async values => {
            let request = {
                Email: values.email,
                FirstName: values.firstName,
                LastName: values.lastName,
                UserName: values.userName,
                PhoneNumber: values.phoneNumber,
                Password: values.password,
                PasswordConfirm: values.passwordConfirm,
                DateOfBirth: values.dateOfBirth
            }

            console.log(request);

            await axiosInstance
                .post(
                    '/Account/SignUp',
                    request
                )
                .then(response => {
                    console.log(response);
                })
                .catch(error => {
                    console.log(error);
                })
        }
    })

    return(
        <form onSubmit={formik.handleSubmit}>
            <label htmlFor='email'>Email</label>
            <input
                id='email'
                name='email'
                type='email'
                onChange={formik.handleChange}
                value={formik.values.email}
            />

            <label htmlFor="firstName">First Name</label>
            <input
                id='firstName'
                name='firstName'
                type='text'
                onChange={formik.handleChange}
                value={formik.values.firstName}
            />

            <label htmlFor="lastName">Last Name</label>
            <input
                id='lastName'
                name='lastName'
                type='text'
                onChange={formik.handleChange}
                value={formik.values.lastName}
            />

            <label htmlFor="userName">User Name</label>
            <input
                id='userName'
                name='userName'
                type='text'
                onChange={formik.handleChange}
                value={formik.values.userName}
            />

            <label htmlFor="phoneNumber">Phone</label>
            <input
                id='phoneNumber'
                name='phoneNumber'
                type='text'
                onChange={formik.handleChange}
                value={formik.values.phoneNumber}
            />

            <label htmlFor="password">Password</label>
            <input
                id='password'
                name='password'
                type='password'
                onChange={formik.handleChange}
                value={formik.values.password}
            />

            <label htmlFor="passwordConfirm">Password Confirm</label>
            <input
                id='passwordConfirm'
                name='passwordConfirm'
                type='password'
                onChange={formik.handleChange}
                value={formik.values.passwordConfirm}
            />

            <label htmlFor="dateOfBirth">Date of Birth</label>
            <input
                id='dateOfBirth'
                name='dateOfBirth'
                type='date'
                onChange={formik.handleChange}
                value={formik.values.dateOfBirth}
            />

            <button type='submit'>Sign Up</button>
        </form>
    )
}

export default SignUp;