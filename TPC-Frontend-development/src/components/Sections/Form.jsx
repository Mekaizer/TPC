import React, { useState } from 'react'
import { Form, Input, Button, Select, DatePicker, Steps, Breadcrumb } from 'antd'
import Modalidad from './modalidad'
import Solicitud from './solicitud'
import BackButton from '../Template/Backbutton'
import Request from '../../service/Request'
import useAuthContext from '../../hooks/useAuthContext'
import Confirmacion from './Confirmacion'
import RequestOC from '../../service/RequestOc'

function Create() {
    const { user } = useAuthContext()
    const [form] = Form.useForm()
    const [step, setStep] = useState(0)
    const [data, setData] = useState(null)
    const [modalidadState, setModalidadState] = useState(null)
    const [tipoSolicitud, setTipoSolicitud] = useState(null)
    const [loadingForm, setLoadingForm] = useState(false)
    const [succesfullData, setSuccesfullData] = useState(null)

    const nextStep = async () => {
        await form.validateFields()
            .then(response => setStep(step + 1))
            .catch(error => console.log(error))
    }

    const previousStep = () => setStep(step - 1)

    const onFinish = async (values) => {

        try {
            const values = await form.validateFields()



            if (tipoSolicitud === 1) {

                RequestOC.post({
                    solped: values.solped ? values.solped : 0,
                    Id_Proveedor: values.proveedor ? values.proveedor.toString() : 0,
                    Id_OE: values.ordenEstadistica ? values.ordenEstadistica.toString() : 0,
                    Estado: 'Recibido',
                    Id_Usuario: user.id_Usuario.toString(),
                    Detalle: values.detalle ? values.detalle : '',
                })
                    .then(response => {
                        if (response.iD_Ticket) {
                            setStep(step + 1)
                            setSuccesfullData(response)
                            form.resetFields()
                        }
                    })
            }

            //Creacion Cotizacion sin solped
            if (tipoSolicitud === 2) {
                Request.post({
                    Id_Solicitante: user.id_Usuario.toString(),
                    Fecha_Creacion_Cotizacion: new Date(),
                    ID_Bien_Servicio: values.bienServicio ? values.bienServicio.toString() : 0,
                    Estado: 'Cotizacion Recibida',
                    Detalle: values.detalle ? values.detalle : '',
                    solped: values.solped ? values.solped : 0,
                })
                    .then(response => {
                        if (response.iD_Cotizacion) {
                            setStep(step + 1)
                            setSuccesfullData(response)
                            form.resetFields()
                        }
                    })
            }


        } catch (error) {
            console.log(error)
        }
    }
    const stepProps = { Form, nextStep, previousStep, data, setData, loadingForm, setModalidadState, modalidadState, tipoSolicitud, setTipoSolicitud, onFinish }


    return (
        <div className="bg-white lg:shadow p-5 lg:p-5 lg:rounded flex flex-col items-center justify-center">

            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: 'Crear solicitud',
                    },
                    {
                        title: 'Finalizar',
                    }
                ]}

                className='text-sm mb-4'
            />

            <h1>Nueva solicitud</h1>

            <div className="w-full lg:w-2/4 mt-8">
                <Form name='solicitud' form={form} className='flex flex-col gap-5' disabled={loadingForm}>
                    <Steps current={step} responsive={false} items={[
                        { title: 'Modalidad' },
                        { title: 'Tipo de solicitud' },
                    ]} />
                    {
                        step === 0 &&
                        <Solicitud {...stepProps} />
                    }
                    {
                        step === 1 &&
                        <Modalidad {...stepProps} />
                    }
                    {
                        step === 2 &&
                        <Confirmacion data={succesfullData} />
                    }
                </Form>


            </div>
        </div>
    )
}

export default Create