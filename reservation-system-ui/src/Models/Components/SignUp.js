import React from 'react';
import { useFormik } from 'formik';
import axiosInstance from '../../Common/API';

const SignUp = () => {
    const formik = useFormik({
        initialValues: {
            userName: '',
            email: '',
            firstMame: '',
            lastName: '',
            phoneNumber: ''
        },
        onSubmit: values => {
            axiosInstance.post(
                '/Account/SignUp',
                {
                    "email": values.email,
                    "firstName": values.firstMame,
                    "lastName": values.lastName
                }
            )
        }
    });

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
                value={formik.values.firstMame}
            />

            <label htmlFor="lastName">Last Name</label>
            <input
                id='lastName'
                name='lastName'
                type='text'
                onChange={formik.handleChange}
                value={formik.values.lastName}
            />

            <button type='submit'>Sign Up</button>
        </form>
    )
}

export default SignUp;