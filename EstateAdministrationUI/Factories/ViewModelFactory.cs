﻿namespace EstateAdministrationUI.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Areas.Estate.Models;
    using BusinessLogic.Models;
    using Microsoft.EntityFrameworkCore.Internal;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateAdministrationUI.Factories.IViewModelFactory" />
    public class ViewModelFactory : IViewModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estateModel">The estate model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">estateModel</exception>
        public EstateViewModel ConvertFrom(EstateModel estateModel)
        {
            if (estateModel == null)
            {
                throw new ArgumentNullException(nameof(estateModel));
            }

            EstateViewModel viewModel = new EstateViewModel
                                        {
                                            EstateName = estateModel.EstateName,
                                            EstateId = estateModel.EstateId
                                        };

            return viewModel;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="createOperatorViewModel">The create operator view model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">createOperatorViewModel</exception>
        public CreateOperatorModel ConvertFrom(CreateOperatorViewModel createOperatorViewModel)
        {
            if (createOperatorViewModel == null)
            {
                throw new ArgumentNullException(nameof(createOperatorViewModel));
            }

            CreateOperatorModel createOperatorModel = new CreateOperatorModel
                                                      {
                                                          RequireCustomMerchantNumber = createOperatorViewModel.RequireCustomMerchantNumber,
                                                          RequireCustomTerminalNumber = createOperatorViewModel.RequireCustomTerminalNumber,
                                                          OperatorName = createOperatorViewModel.OperatorName
                                                      };

            return createOperatorModel;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="createContractViewModel">The create contract view model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">createContractViewModel</exception>
        public CreateContractModel ConvertFrom(CreateContractViewModel createContractViewModel)
        {
            if (createContractViewModel == null)
            {
                throw new ArgumentNullException(nameof(createContractViewModel));
            }

            CreateContractModel createContractModel = new CreateContractModel
                                                      {
                                                          OperatorId = createContractViewModel.OperatorId,
                                                          Description = createContractViewModel.ContractDescription
                                                      };

            return createContractModel;
        }

        public List<OperatorListViewModel> ConvertFrom(Guid estateId,
                                                       List<EstateOperatorModel> estateOperatorModels)
        {
            if (estateOperatorModels == null || EnumerableExtensions.Any(estateOperatorModels) == false)
            {
                throw new ArgumentNullException(nameof(estateOperatorModels));
            }

            List<OperatorListViewModel> viewModels = new List<OperatorListViewModel>();

            estateOperatorModels.ForEach(eo => viewModels.Add(this.ConvertFrom(estateId, eo)));

            return viewModels;
        }

        public OperatorListViewModel ConvertFrom(Guid estateId,
                                                 EstateOperatorModel estateOperatorModel)
        {
            if (estateOperatorModel == null)
            {
                throw new ArgumentNullException(nameof(estateOperatorModel));
            }

            OperatorListViewModel viewModel = new OperatorListViewModel
                                              {
                                                  EstateId = estateId,
                                                  OperatorId = estateOperatorModel.OperatorId,
                                                  OperatorName = estateOperatorModel.Name,
                                                  RequireCustomMerchantNumber = estateOperatorModel.RequireCustomMerchantNumber,
                                                  RequireCustomTerminalNumber = estateOperatorModel.RequireCustomTerminalNumber
                                              };

            return viewModel;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="createMerchantViewModel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">createMerchantViewModel</exception>
        public CreateMerchantModel ConvertFrom(CreateMerchantViewModel createMerchantViewModel)
        {
            if (createMerchantViewModel == null)
            {
                throw new ArgumentNullException(nameof(createMerchantViewModel));
            }

            CreateMerchantModel createMerchantModel = new CreateMerchantModel
                                                      {
                                                          Address = new AddressModel
                                                                    {
                                                                        AddressLine1 = createMerchantViewModel.AddressLine1,
                                                                        AddressLine2 = createMerchantViewModel.AddressLine2,
                                                                        AddressLine3 = createMerchantViewModel.AddressLine3,
                                                                        AddressLine4 = createMerchantViewModel.AddressLine4,
                                                                        Country = createMerchantViewModel.Country,
                                                                        PostalCode = createMerchantViewModel.PostalCode,
                                                                        Region = createMerchantViewModel.Region,
                                                                        Town = createMerchantViewModel.Town,
                                                                    },
                                                          Contact = new ContactModel
                                                                    {
                                                                        ContactPhoneNumber = createMerchantViewModel.ContactPhoneNumber,
                                                                        ContactName = createMerchantViewModel.ContactName,
                                                                        ContactEmailAddress = createMerchantViewModel.ContactEmailAddress
                                                                    },
                                                          MerchantName = createMerchantViewModel.MerchantName
                                                      };

            return createMerchantModel;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchantModels">The merchant models.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">merchantModels</exception>
        public List<MerchantListViewModel> ConvertFrom(List<MerchantModel> merchantModels)
        {
            if (merchantModels == null || EnumerableExtensions.Any(merchantModels) == false)
            {
                throw new ArgumentNullException(nameof(merchantModels));
            }

            List<MerchantListViewModel> viewModels = new List<MerchantListViewModel>();

            foreach (MerchantModel merchantModel in merchantModels)
            {
                viewModels.Add(new MerchantListViewModel
                               {
                                   AddressLine1 = merchantModel.Addresses == null ? string.Empty :
                                       merchantModel.Addresses.FirstOrDefault() == null ? string.Empty : merchantModel.Addresses.First().AddressLine1,
                                   MerchantId = merchantModel.MerchantId,
                                   ContactName = merchantModel.Contacts == null ? string.Empty :
                                       merchantModel.Contacts.FirstOrDefault() == null ? string.Empty : merchantModel.Contacts.First().ContactName,
                                   Town = merchantModel.Addresses == null ? string.Empty :
                                       merchantModel.Addresses.FirstOrDefault() == null ? string.Empty : merchantModel.Addresses.First().Town,
                                   MerchantName = merchantModel.MerchantName,
                                   EstateId = merchantModel.EstateId,
                                   NumberOfDevices = merchantModel.Devices != null && merchantModel.Devices.Any() ? merchantModel.Devices.Count : 0,
                                   NumberOfOperators = merchantModel.Operators == null ? 0 :
                                       merchantModel.Operators != null && merchantModel.Operators.Any() ? merchantModel.Operators.Count : 0,
                                   NumberOfUsers = 0
                               });
            }

            return viewModels;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="contractModels">The contract models.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">contractModels</exception>
        public List<ContractListViewModel> ConvertFrom(List<ContractModel> contractModels)
        {
            if (contractModels == null)
            {
                throw new ArgumentNullException(nameof(contractModels));
            }

            List<ContractListViewModel> viewModels = new List<ContractListViewModel>();

            foreach (ContractModel contractModel in contractModels)
            {
                viewModels.Add(new ContractListViewModel
                               {
                                   EstateId = contractModel.EstateId,
                                   OperatorName = contractModel.OperatorName,
                                   ContractId = contractModel.ContractId,
                                   Description = contractModel.Description,
                                   OperatorId = contractModel.OperatorId,
                                   NumberOfProducts = contractModel.NumberOfProducts
                               });
            }

            return viewModels;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchantModel">The merchant model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">merchantModel</exception>
        public MerchantViewModel ConvertFrom(MerchantModel merchantModel)
        {
            if (merchantModel == null)
            {
                throw new ArgumentNullException(nameof(merchantModel));
            }

            MerchantViewModel viewModel = new MerchantViewModel();

            viewModel.EstateId = merchantModel.EstateId;
            viewModel.MerchantId = merchantModel.MerchantId;
            viewModel.MerchantName = merchantModel.MerchantName;
            viewModel.Balance = merchantModel.Balance;
            viewModel.AvailableBalance = merchantModel.AvailableBalance;
            viewModel.Addresses = this.ConvertFrom(merchantModel.Addresses);
            viewModel.Contacts = this.ConvertFrom(merchantModel.Contacts);
            viewModel.Operators = this.ConvertFrom(merchantModel.Operators);
            viewModel.Devices = this.ConvertFrom(merchantModel.Devices);

            return viewModel;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="makeMerchantDepositViewModel">The make merchant deposit view model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">makeMerchantDepositViewModel</exception>
        public MakeMerchantDepositModel ConvertFrom(MakeMerchantDepositViewModel makeMerchantDepositViewModel)
        {
            if (makeMerchantDepositViewModel == null)
            {
                throw new ArgumentNullException(nameof(makeMerchantDepositViewModel));
            }

            MakeMerchantDepositModel makeMerchantDepositModel = new MakeMerchantDepositModel();

            makeMerchantDepositModel.DepositDateTime = DateTime.ParseExact(makeMerchantDepositViewModel.DepositDate, "dd/MM/yyyy", null);
            makeMerchantDepositModel.Amount = decimal.Parse(makeMerchantDepositViewModel.Amount);
            makeMerchantDepositModel.Reference = makeMerchantDepositViewModel.Reference;
            makeMerchantDepositModel.MerchantId = Guid.Parse(makeMerchantDepositViewModel.MerchantId);

            return makeMerchantDepositModel;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="deviceModels">The device models.</param>
        /// <returns></returns>
        private Dictionary<String, String> ConvertFrom(Dictionary<Guid, String> deviceModels)
        {
            Dictionary<String, String> viewModels = new Dictionary<String, String>();

            if (deviceModels == null || deviceModels.Any() == false)
            {
                return viewModels;
            }

            foreach (KeyValuePair<Guid, String> model in deviceModels)
            {
                viewModels.Add(model.Key.ToString(), model.Value);
            }

            return viewModels;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="addressModels">The address models.</param>
        /// <returns></returns>
        private List<AddressViewModel> ConvertFrom(List<AddressModel> addressModels)
        {
            List<AddressViewModel> viewModels = new List<AddressViewModel>();

            if (addressModels == null || addressModels.Any() == false)
            {
                return viewModels;
            }

            foreach (AddressModel model in addressModels)
            {
                viewModels.Add(this.ConvertFrom(model));
            }

            return viewModels;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="addressModel">The address model.</param>
        /// <returns></returns>
        private AddressViewModel ConvertFrom(AddressModel addressModel)
        {
            return new AddressViewModel
                   {
                       AddressId = addressModel.AddressId,
                       AddressLine1 = addressModel.AddressLine1,
                       AddressLine2 = addressModel.AddressLine2,
                       AddressLine3 = addressModel.AddressLine3,
                       AddressLine4 = addressModel.AddressLine4,
                       Country = addressModel.Country,
                       PostalCode = addressModel.PostalCode,
                       Region = addressModel.Region,
                       Town = addressModel.Town
                   };
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="contactModels">The contact models.</param>
        /// <returns></returns>
        private List<ContactViewModel> ConvertFrom(List<ContactModel> contactModels)
        {
            List<ContactViewModel> viewModels = new List<ContactViewModel>();

            if (contactModels == null)
            {
                return viewModels;
            }

            foreach (ContactModel model in contactModels)
            {
                viewModels.Add(this.ConvertFrom(model));
            }

            return viewModels;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="contactModel">The contact model.</param>
        /// <returns></returns>
        private ContactViewModel ConvertFrom(ContactModel contactModel)
        {
            return new ContactViewModel
                   {
                       ContactEmailAddress = contactModel.ContactEmailAddress,
                       ContactId = contactModel.ContactId,
                       ContactName = contactModel.ContactName,
                       ContactPhoneNumber = contactModel.ContactPhoneNumber
                   };
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="operatorModels">The operator models.</param>
        /// <returns></returns>
        private List<MerchantOperatorViewModel> ConvertFrom(List<MerchantOperatorModel> operatorModels)
        {
            List<MerchantOperatorViewModel> viewModels = new List<MerchantOperatorViewModel>();

            if (operatorModels == null || operatorModels.Any() == false)
            {
                return viewModels;
            }

            foreach (MerchantOperatorModel model in operatorModels)
            {
                viewModels.Add(this.ConvertFrom(model));
            }

            return viewModels;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="operatorModel">The operator model.</param>
        /// <returns></returns>
        private MerchantOperatorViewModel ConvertFrom(MerchantOperatorModel operatorModel)
        {
            return new MerchantOperatorViewModel
                   {
                       MerchantNumber = operatorModel.MerchantNumber,
                       Name = operatorModel.Name,
                       OperatorId = operatorModel.OperatorId,
                       TerminalNumber = operatorModel.TerminalNumber
                   };
        }

        #endregion
    }
}