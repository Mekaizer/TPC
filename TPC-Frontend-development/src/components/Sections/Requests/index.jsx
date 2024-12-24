import React, { useState } from 'react'
import Table from '../../Template/Table'
import { Breadcrumb } from 'antd'
import Actions from './Actions'
import Cotizar from './Cotizar'
import { normalizeText } from '../../../utils/paragraph'
import Search from './Search'
import { isNotEmpty } from '../../../utils/validations'
import useAuthContext from '../../../hooks/useAuthContext'
import useRequest from '../../../hooks/useRequest'
import Excel from './Excel'

function Request() {

    const { user } = useAuthContext()

    const { data, isLoading, isError, error, isSuccess, refetch } = useRequest()

    const [search, setSearch] = useState({
        data: [],
        ticket: ''
    })



    const handleSearch = e => {
        const ticket = normalizeText(e.target.value)
        const result = data.filter(t => normalizeText(t.iD_Cotizacion.toString()).includes(ticket) || normalizeText(t.estado).includes(ticket) || normalizeText(t.solped.toString()).includes(ticket) || normalizeText(t.detalle).includes(ticket) || normalizeText(t.iD_Bien_Servicio).includes(ticket))
        setSearch({
            data: result,
            ticket
        })
    }

    const columns = [
        { title: 'N° Cotizacion', dataIndex: 'iD_Cotizacion', key: 'name', align: 'left', responsive: ['md'] },
        {
            title: 'Tipo de solicitud', key: 'type', align: 'left', responsive: ['md'], render: (text, record) => {
                return record.solped == 0 ? 'Solicitud sin Solped' : 'Solicitud de cotizacion con solped'
            }
        },
        { title: 'Estado', dataIndex: 'estado', key: 'status', align: 'center' },
        { title: 'Fecha', dataIndex: 'fecha_Creacion_Cotizacion', key: 'date', align: 'center', responsive: ['md'], render: (text, record) => new Date(record.fecha_Creacion_Cotizacion).toLocaleDateString() },
        {
            title: 'Nº Solped', dataIndex: 'solped', key: 'solped', align: 'center', render: (text, record) => {
                return record.solped == 0 ? 'Sin solped' : record.solped
            }
        },
        { title: 'Detalle', dataIndex: 'detalle', key: 'detail', align: 'center', render: (text, record) => record.solped != 0 ? 'Sin detalle' : record.detalle },
        { title: 'Bien/Servicio', dataIndex: 'bien_Servicio', key: 'bien_servicio', align: 'center' },
        {
            title: 'Acciones', dataIndex: 'actions', key: 'actions', align: 'center', render: (text, record) => <Actions Solicitud={record} refetch={refetch} />
        },
    ]

    if (user.isAdmin) {

        columns.push(
            {
                title: 'Cotizar', dataIndex: 'cotizar', key: 'cotizar', align: 'center', render: (text, record) => <Cotizar cotizacion={record} />
            }
        )
    }

    return (
        <div>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: 'Solicitudes de cotizacion',
                    },
                ]}

                className='text-sm mb-4'
            />

            <div className='flex justify-between flex-col lg:flex-row items-center mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Solicitudes de cotizacion
                </p>
            </div>
            <div className="flex flex-col lg:flex-row items-center gap-2 mb-4">
                <div className="flex-1 order-1">
                </div>
                {
                    user?.isAdmin &&
                    <div className="flex gap-x-2 order-2">
                        <Excel />
                    </div>
                }

                <Search onChange={handleSearch} />
            </div>

            <Table
                columns={columns}
                data={isSuccess ? (isNotEmpty(search.ticket) ? search.data :
                    user.isAdmin ? data : data.filter(t => t.idS == user.id_Usuario)
                ) : []}
            />
        </div>
    )
}

export default Request