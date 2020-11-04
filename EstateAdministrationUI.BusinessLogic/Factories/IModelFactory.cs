﻿namespace EstateAdministrationUI.BusinessLogic.Factories
{
    using System.Collections.Generic;
    using EstateManagement.DataTransferObjects.Requests;
    using EstateManagement.DataTransferObjects.Responses;
    using EstateReporting.DataTransferObjects;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        EstateModel ConvertFrom(EstateResponse source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        CreateOperatorRequest ConvertFrom(CreateOperatorModel source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        CreateContractRequest ConvertFrom(CreateContractModel source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        CreateOperatorResponseModel ConvertFrom(CreateOperatorResponse source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        List<MerchantModel> ConvertFrom(List<MerchantResponse> source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        List<ContractModel> ConvertFrom(List<ContractResponse> source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        ContractModel ConvertFrom(ContractResponse source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        MerchantModel ConvertFrom(MerchantResponse source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        CreateMerchantResponseModel ConvertFrom(CreateMerchantResponse source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        CreateMerchantRequest ConvertFrom(CreateMerchantModel source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        MakeMerchantDepositRequest ConvertFrom(MakeMerchantDepositModel source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        MakeMerchantDepositResponseModel ConvertFrom(MakeMerchantDepositResponse source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        CreateContractResponseModel ConvertFrom(CreateContractResponse source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        AddProductToContractRequest ConvertFrom(AddProductToContractModel source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        AddProductToContractResponseModel ConvertFrom(AddProductToContractResponse source);

        #endregion

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        AddTransactionFeeForProductToContractRequest ConvertFrom(AddTransactionFeeToContractProductModel source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        AddTransactionFeeToContractProductResponseModel ConvertFrom(AddTransactionFeeForProductToContractResponse source);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        TransactionForPeriodModel ConvertToPeriodModel(TransactionsByDayResponse source);

        TransactionsByDateModel ConvertFrom(TransactionsByDayResponse source);

        TransactionsByWeekModel ConvertFrom(TransactionsByWeekResponse source);

        TransactionsByMonthModel ConvertFrom(TransactionsByMonthResponse source);
    }
}