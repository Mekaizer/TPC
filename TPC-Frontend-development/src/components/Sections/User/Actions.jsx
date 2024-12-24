import React from 'react'
import Edit from './Edit'
import Delete from './Delete'
import useAuthContext from '../../../hooks/useAuthContext'


function Actions({ user, refetch }) {

    const dataUser = useAuthContext()

    console.log( dataUser.user.isAdmin )
    return (
        <div className='flex items-center justify-center gap-2'>
            {
                (dataUser.user.isAdmin ||( dataUser.user.id_Usuario == user.id_Usuario) )&&
                
                <Edit user={user} refetch={refetch} />
            }
            
            {
                dataUser.user.isAdmin&&
                <Delete user={user} refetch={refetch} />

            }
        </div>
    )
}

export default Actions