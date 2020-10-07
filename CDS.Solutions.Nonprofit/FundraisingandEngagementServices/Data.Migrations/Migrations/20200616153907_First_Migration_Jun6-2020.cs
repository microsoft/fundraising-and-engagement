using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundraisingandEngagement.Data.Migrations
{
    public partial class First_Migration_Jun62020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "BankRun",
                schema: "dbo",
                columns: table => new
                {
                    BankRunId = table.Column<Guid>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    PaymentProcessorId = table.Column<Guid>(nullable: true),
                    AccountToCreditId = table.Column<Guid>(nullable: true),
                    BankRunStatus = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    DateToBeProcessed = table.Column<DateTime>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    FileCreationNumber = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankRun", x => x.BankRunId);
                });

            migrationBuilder.CreateTable(
                name: "BankRunSchedule",
                schema: "dbo",
                columns: table => new
                {
                    BankRunScheduleId = table.Column<Guid>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    PaymentScheduleId = table.Column<Guid>(nullable: true),
                    BankRunId = table.Column<Guid>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankRunSchedule", x => x.BankRunScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Designation",
                schema: "dbo",
                columns: table => new
                {
                    DesignationId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(maxLength: 160, nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    StatusReason = table.Column<int>(nullable: true),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.DesignationId);
                });

            migrationBuilder.CreateTable(
                name: "EventTable",
                schema: "dbo",
                columns: table => new
                {
                    eventtableid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    identifier = table.Column<string>(maxLength: 100, nullable: true),
                    tablecapacity = table.Column<string>(maxLength: 100, nullable: true),
                    tablenumber = table.Column<string>(maxLength: 100, nullable: true),
                    eventid = table.Column<Guid>(nullable: false),
                    eventticketid = table.Column<Guid>(nullable: false),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTable", x => x.eventtableid);
                });

            migrationBuilder.CreateTable(
                name: "GiftAidDeclaration",
                schema: "dbo",
                columns: table => new
                {
                    GiftAidDeclarationId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    DeclarationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeclarationDelivered = table.Column<int>(maxLength: 100, nullable: true),
                    GiftAidDeclarationHtml = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 150, nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftAidDeclaration", x => x.GiftAidDeclarationId);
                });

            migrationBuilder.CreateTable(
                name: "MembershipGroup",
                schema: "dbo",
                columns: table => new
                {
                    MembershipGroupId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GroupName = table.Column<string>(maxLength: 100, nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipGroup", x => x.MembershipGroupId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentProcessor",
                schema: "dbo",
                columns: table => new
                {
                    PaymentProcessorId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BankRunFileFormat = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentGatewayType = table.Column<int>(nullable: false),
                    AdyenMerchantAccount = table.Column<string>(maxLength: 100, nullable: true),
                    AdyenUsername = table.Column<string>(maxLength: 100, nullable: true),
                    AdyenPassword = table.Column<string>(maxLength: 100, nullable: true),
                    AdyenUrl = table.Column<string>(nullable: true),
                    AdyenCheckoutUrl = table.Column<string>(maxLength: 100, nullable: true),
                    IatsAgentCode = table.Column<string>(maxLength: 100, nullable: true),
                    IatsPassword = table.Column<string>(maxLength: 100, nullable: true),
                    MonerisStoreId = table.Column<string>(maxLength: 100, nullable: true),
                    MonerisApiKey = table.Column<string>(maxLength: 100, nullable: true),
                    MonerisTestMode = table.Column<bool>(nullable: true),
                    StripeServiceKey = table.Column<string>(maxLength: 100, nullable: true),
                    WorldPayServiceKey = table.Column<string>(maxLength: 100, nullable: true),
                    WorldPayClientKey = table.Column<string>(nullable: true),
                    ScotiabankCustomerNumber = table.Column<string>(maxLength: 10, nullable: true),
                    OriginatorShortName = table.Column<string>(maxLength: 15, nullable: true),
                    OriginatorLongName = table.Column<string>(maxLength: 30, nullable: true),
                    BmoOriginatorId = table.Column<string>(maxLength: 10, nullable: true),
                    AbaRemitterName = table.Column<string>(maxLength: 16, nullable: true),
                    AbaUserName = table.Column<string>(maxLength: 26, nullable: true),
                    AbaUserNumber = table.Column<string>(maxLength: 6, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentProcessor", x => x.PaymentProcessorId);
                });

            migrationBuilder.CreateTable(
                name: "PreferenceCategory",
                schema: "dbo",
                columns: table => new
                {
                    preferencecategoryid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 100, nullable: true),
                    categorycode = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferenceCategory", x => x.preferencecategoryid);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptTemplate",
                schema: "dbo",
                columns: table => new
                {
                    ReceiptTemplateId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Identifier = table.Column<string>(maxLength: 50, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 150, nullable: true),
                    HTMLReceipt = table.Column<string>(nullable: true),
                    HTMLAcknowledgement = table.Column<string>(nullable: true),
                    HeaderImage = table.Column<string>(maxLength: 50, nullable: true),
                    FooterImage = table.Column<string>(maxLength: 50, nullable: true),
                    SignatureImage = table.Column<string>(maxLength: 50, nullable: true),
                    PreferredLanguage = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    TemplateTypeCode = table.Column<int>(nullable: true),
                    OwningBusinessUnitId = table.Column<Guid>(nullable: true),
                    EmailHtmlBodyTemplate = table.Column<string>(nullable: true),
                    EmailTextBodyTemplate = table.Column<string>(nullable: true),
                    DocxTemplate = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptTemplate", x => x.ReceiptTemplateId);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationPreference",
                schema: "dbo",
                columns: table => new
                {
                    registrationpreferenceid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    registrationid = table.Column<Guid>(nullable: false),
                    eventpreference = table.Column<Guid>(nullable: false),
                    eventid = table.Column<Guid>(nullable: false),
                    other = table.Column<string>(maxLength: 150, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationPreference", x => x.registrationpreferenceid);
                });

            migrationBuilder.CreateTable(
                name: "RelatedImage",
                schema: "dbo",
                columns: table => new
                {
                    RelatedImageId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SmallImage = table.Column<string>(maxLength: 100, nullable: true),
                    LastPublished = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedImage", x => x.RelatedImageId);
                });

            migrationBuilder.CreateTable(
                name: "TermsOfReference",
                schema: "dbo",
                columns: table => new
                {
                    TermsOfReferenceId = table.Column<Guid>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CcvMessage = table.Column<string>(nullable: true),
                    CoverCostsMessage = table.Column<string>(nullable: true),
                    FailureMessage = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true),
                    GiftAidAcceptence = table.Column<string>(nullable: true),
                    GiftAidDeclaration = table.Column<string>(nullable: true),
                    GiftAidDetails = table.Column<string>(nullable: true),
                    PrivacyPolicy = table.Column<string>(nullable: true),
                    PrivacyUrl = table.Column<string>(nullable: true),
                    ShowPrivacy = table.Column<bool>(nullable: true),
                    ShowTermsConditions = table.Column<bool>(nullable: true),
                    Signup = table.Column<string>(nullable: true),
                    TermsConditions = table.Column<string>(nullable: true),
                    TermsConditionsUrl = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsOfReference", x => x.TermsOfReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCurrency",
                schema: "dbo",
                columns: table => new
                {
                    TransactionCurrencyId = table.Column<Guid>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CurrencyName = table.Column<string>(maxLength: 150, nullable: true),
                    CurrencySymbol = table.Column<string>(maxLength: 150, nullable: true),
                    IsoCurrencyCode = table.Column<string>(maxLength: 150, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(8, 2)", nullable: true),
                    IsBase = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCurrency", x => x.TransactionCurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "TributeOrMemory",
                schema: "dbo",
                columns: table => new
                {
                    TributeOrMemoryId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    TributeOrMemoryTypeCode = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TributeOrMemory", x => x.TributeOrMemoryId);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                schema: "dbo",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Document = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileSize = table.Column<int>(nullable: true),
                    IsDocument = table.Column<bool>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RegardingObjectId = table.Column<Guid>(nullable: true),
                    ObjectType = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    MimeType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Note_BankRun_RegardingObjectId",
                        column: x => x.RegardingObjectId,
                        principalSchema: "dbo",
                        principalTable: "BankRun",
                        principalColumn: "BankRunId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                schema: "dbo",
                columns: table => new
                {
                    ConfigurationId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TeamOwnerId = table.Column<Guid>(nullable: true),
                    PaymentProcessorId = table.Column<Guid>(nullable: true),
                    AddressAuth1 = table.Column<string>(maxLength: 100, nullable: true),
                    AddressAuth2 = table.Column<string>(maxLength: 100, nullable: true),
                    AzureWebApiUrl = table.Column<string>(maxLength: 100, nullable: true),
                    BankRunPregeneratedBy = table.Column<int>(nullable: true),
                    CharityTitle = table.Column<string>(maxLength: 100, nullable: true),
                    AzureWebApp = table.Column<string>(maxLength: 100, nullable: true),
                    ScheMaxRetries = table.Column<int>(nullable: true),
                    ScheRecurrenceStart = table.Column<int>(nullable: true),
                    ScheRetryinterval = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    DefaultConfiguration = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.ConfigurationId);
                    table.ForeignKey(
                        name: "FK__Configura__Payme__5CA1C101",
                        column: x => x.PaymentProcessorId,
                        principalSchema: "dbo",
                        principalTable: "PaymentProcessor",
                        principalColumn: "PaymentProcessorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                schema: "dbo",
                columns: table => new
                {
                    PaymentMethodId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    PaymentProcessorId = table.Column<Guid>(nullable: true),
                    TransactionFraudCode = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionResult = table.Column<string>(maxLength: 100, nullable: true),
                    Emailaddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone1 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingStateorProvince = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine3 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCity = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCountry = table.Column<string>(maxLength: 100, nullable: true),
                    BillingPostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    CcLast4 = table.Column<string>(maxLength: 100, nullable: true),
                    CcExpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CcExpMmYy = table.Column<string>(maxLength: 100, nullable: true),
                    CcBrandCode = table.Column<int>(nullable: true),
                    BankName = table.Column<string>(maxLength: 100, nullable: true),
                    BankActNumber = table.Column<string>(maxLength: 100, nullable: true),
                    BankActRtNumber = table.Column<string>(maxLength: 100, nullable: true),
                    BankTypeCode = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    IsReusable = table.Column<bool>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    NameOnFile = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    StripeCustomerId = table.Column<string>(nullable: true),
                    AuthToken = table.Column<string>(nullable: true),
                    AbaFinancialInstitutionName = table.Column<string>(maxLength: 3, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.PaymentMethodId);
                    table.ForeignKey(
                        name: "FK__PaymentMe__Payme__56E8E7AB",
                        column: x => x.PaymentProcessorId,
                        principalSchema: "dbo",
                        principalTable: "PaymentProcessor",
                        principalColumn: "PaymentProcessorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventPreference",
                schema: "dbo",
                columns: table => new
                {
                    eventpreferenceid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    identifier = table.Column<string>(maxLength: 100, nullable: true),
                    eventid = table.Column<Guid>(nullable: false),
                    preferenceid = table.Column<Guid>(nullable: false),
                    preferencecategoryid = table.Column<Guid>(nullable: false),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPreference", x => x.eventpreferenceid);
                    table.ForeignKey(
                        name: "FK__EventPreference__PreferenceCategory__18B6AB52",
                        column: x => x.preferencecategoryid,
                        principalSchema: "dbo",
                        principalTable: "PreferenceCategory",
                        principalColumn: "preferencecategoryid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preference",
                schema: "dbo",
                columns: table => new
                {
                    preferenceid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    preferencecategoryid = table.Column<Guid>(nullable: true),
                    name = table.Column<string>(maxLength: 150, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.preferenceid);
                    table.ForeignKey(
                        name: "FK__Preference__PreferenceCategory__18B6AB51",
                        column: x => x.preferencecategoryid,
                        principalSchema: "dbo",
                        principalTable: "PreferenceCategory",
                        principalColumn: "preferencecategoryid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MembershipCategory",
                schema: "dbo",
                columns: table => new
                {
                    MembershipCategoryId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    AmountMembership = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    GoodWillDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembershipDuration = table.Column<int>(nullable: true),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipCategory", x => x.MembershipCategoryId);
                    table.ForeignKey(
                        name: "FK_MembershipCategory_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "dbo",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    PaymentProcessorId = table.Column<Guid>(nullable: true),
                    CampaignId = table.Column<Guid>(nullable: true),
                    AppealId = table.Column<Guid>(nullable: true),
                    PackageId = table.Column<Guid>(nullable: true),
                    DesignationId = table.Column<Guid>(nullable: true),
                    ConfigurationId = table.Column<Guid>(nullable: true),
                    VenueId = table.Column<Guid>(nullable: true),
                    TeamOwnerId = table.Column<Guid>(nullable: true),
                    TermsOfReferenceId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Goal = table.Column<decimal>(type: "money", nullable: true),
                    Capacity = table.Column<int>(nullable: true),
                    Coordinator = table.Column<string>(nullable: true),
                    TimeAndDate = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Sponsorship = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    EventTypeCode = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    MapLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    MapLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    MapLine3 = table.Column<string>(maxLength: 100, nullable: true),
                    MapCity = table.Column<string>(maxLength: 100, nullable: true),
                    MapStateOrProvince = table.Column<string>(maxLength: 100, nullable: true),
                    MapPostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    MapCountry = table.Column<string>(maxLength: 100, nullable: true),
                    ProposedEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProposedStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShowMap = table.Column<bool>(nullable: true),
                    FreeEvent = table.Column<bool>(nullable: true),
                    LargeImage = table.Column<string>(maxLength: 100, nullable: true),
                    SmallImage = table.Column<string>(maxLength: 100, nullable: true),
                    CostAmount = table.Column<decimal>(type: "money", nullable: true),
                    CostPercentage = table.Column<int>(nullable: true),
                    ExternalUrl = table.Column<string>(maxLength: 250, nullable: true),
                    ForceRedirect = table.Column<bool>(nullable: true),
                    ForceRedirectTiming = table.Column<int>(nullable: true),
                    HomePageUrl = table.Column<string>(maxLength: 100, nullable: true),
                    InvoiceMessage = table.Column<string>(nullable: true),
                    LabelLanguageCode = table.Column<string>(maxLength: 100, nullable: true),
                    LastPublished = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MadeVisible = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentNotice = table.Column<string>(nullable: true),
                    Removed = table.Column<bool>(nullable: true),
                    SelectCurrency = table.Column<bool>(nullable: true),
                    SetAcceptNotice = table.Column<bool>(nullable: true),
                    SetCoverCosts = table.Column<bool>(nullable: true),
                    SetSignUp = table.Column<bool>(nullable: true),
                    ShowApple = table.Column<bool>(nullable: true),
                    ShowCompany = table.Column<bool>(nullable: true),
                    ShowCoverCosts = table.Column<bool>(nullable: true),
                    ShowGoogle = table.Column<bool>(nullable: true),
                    ShowInvoice = table.Column<bool>(nullable: true),
                    ShowPayPal = table.Column<bool>(nullable: true),
                    ShowCreditCard = table.Column<bool>(nullable: true),
                    ThankYou = table.Column<string>(nullable: true),
                    Visible = table.Column<bool>(nullable: true),
                    ShowGiftAid = table.Column<bool>(nullable: true),
                    RemovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK__Event__Configura__00DF2177",
                        column: x => x.ConfigurationId,
                        principalSchema: "dbo",
                        principalTable: "Configuration",
                        principalColumn: "ConfigurationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Event__Designation",
                        column: x => x.DesignationId,
                        principalSchema: "dbo",
                        principalTable: "Designation",
                        principalColumn: "DesignationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_PaymentProcessor_PaymentProcessorId",
                        column: x => x.PaymentProcessorId,
                        principalSchema: "dbo",
                        principalTable: "PaymentProcessor",
                        principalColumn: "PaymentProcessorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Event__TermsOfRe__01D345B0",
                        column: x => x.TermsOfReferenceId,
                        principalSchema: "dbo",
                        principalTable: "TermsOfReference",
                        principalColumn: "TermsOfReferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptStack",
                schema: "dbo",
                columns: table => new
                {
                    ReceiptStackId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfigurationId = table.Column<Guid>(nullable: true),
                    CurrentRange = table.Column<double>(nullable: true),
                    NumberRange = table.Column<int>(nullable: true),
                    Prefix = table.Column<string>(maxLength: 100, nullable: true),
                    ReceiptYear = table.Column<int>(nullable: true),
                    StartingRange = table.Column<double>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    OwningBusinessUnitId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptStack", x => x.ReceiptStackId);
                    table.ForeignKey(
                        name: "FK__ReceiptSt__Confi__7B264821",
                        column: x => x.ConfigurationId,
                        principalSchema: "dbo",
                        principalTable: "Configuration",
                        principalColumn: "ConfigurationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                schema: "dbo",
                columns: table => new
                {
                    MembershipId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembershipCategoryId = table.Column<Guid>(nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Primary = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => x.MembershipId);
                    table.ForeignKey(
                        name: "FK__Membershi__Membe__70A8B9AE",
                        column: x => x.MembershipCategoryId,
                        principalSchema: "dbo",
                        principalTable: "MembershipCategory",
                        principalColumn: "MembershipCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MembershipOrder",
                schema: "dbo",
                columns: table => new
                {
                    MembershipOrderId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromMembershipCategoryId = table.Column<Guid>(nullable: true),
                    ToMembershipGroupId = table.Column<Guid>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipOrder", x => x.MembershipOrderId);
                    table.ForeignKey(
                        name: "FK__Membershi__FromM__6EC0713C",
                        column: x => x.FromMembershipCategoryId,
                        principalSchema: "dbo",
                        principalTable: "MembershipCategory",
                        principalColumn: "MembershipCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Membershi__ToMem__6FB49575",
                        column: x => x.ToMembershipGroupId,
                        principalSchema: "dbo",
                        principalTable: "MembershipGroup",
                        principalColumn: "MembershipGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventDisclaimer",
                schema: "dbo",
                columns: table => new
                {
                    EventDisclaimerId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDisclaimer", x => x.EventDisclaimerId);
                    table.ForeignKey(
                        name: "FK__EventDisl__Event__14E61A24",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventDonation",
                schema: "dbo",
                columns: table => new
                {
                    EventDonationId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDonation", x => x.EventDonationId);
                    table.ForeignKey(
                        name: "FK__EventDona__Event__02C769E9",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventDonation_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventPackage",
                schema: "dbo",
                columns: table => new
                {
                    EventPackageId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    CampaignId = table.Column<Guid>(nullable: true),
                    PackageId = table.Column<Guid>(nullable: true),
                    Appealid = table.Column<Guid>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    ConfigurationId = table.Column<Guid>(nullable: true),
                    ConstituentId = table.Column<Guid>(nullable: true),
                    PaymentmethodId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonReceiptable = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountNonreceiptable = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountTax = table.Column<decimal>(type: "money", nullable: true),
                    RefAmount = table.Column<decimal>(type: "money", nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    Emailaddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone1 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone2 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCity = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCountry = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine3 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingPostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    BillingStateorProvince = table.Column<string>(maxLength: 100, nullable: true),
                    ChequeNumber = table.Column<string>(maxLength: 100, nullable: true),
                    ChequeWireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRefunded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEntrySource = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    CcBrandCode = table.Column<int>(nullable: true),
                    OrganizationName = table.Column<string>(maxLength: 100, nullable: true),
                    DataEntryReference = table.Column<string>(maxLength: 100, nullable: true),
                    MissioninvoiceIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionFraudCode = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionResult = table.Column<string>(maxLength: 100, nullable: true),
                    ThirdPartyReceipt = table.Column<string>(maxLength: 100, nullable: true),
                    SumDonations = table.Column<int>(nullable: true),
                    SumProducts = table.Column<int>(nullable: true),
                    SumSponsorships = table.Column<int>(nullable: true),
                    SumTickets = table.Column<int>(nullable: true),
                    SumRegistrations = table.Column<int>(nullable: true),
                    ValDonations = table.Column<decimal>(type: "money", nullable: true),
                    ValProducts = table.Column<decimal>(type: "money", nullable: true),
                    ValSponsorships = table.Column<decimal>(type: "money", nullable: true),
                    ValTickets = table.Column<decimal>(type: "money", nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPackage", x => x.EventPackageId);
                    table.ForeignKey(
                        name: "FK__EventPack__Confi__11158940",
                        column: x => x.ConfigurationId,
                        principalSchema: "dbo",
                        principalTable: "Configuration",
                        principalColumn: "ConfigurationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__EventPack__Event__10216507",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__EventPack__Payme__1209AD79",
                        column: x => x.PaymentmethodId,
                        principalSchema: "dbo",
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventPackage_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventProduct",
                schema: "dbo",
                columns: table => new
                {
                    EventProductId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonReceiptable = table.Column<decimal>(type: "money", nullable: true),
                    MaxProducts = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    ValAvailable = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    RestrictPerRegistration = table.Column<bool>(nullable: true),
                    ValSold = table.Column<int>(nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    SumSold = table.Column<decimal>(type: "money", nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventProduct", x => x.EventProductId);
                    table.ForeignKey(
                        name: "FK__EventProd__Event__09746778",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventProduct_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventSponsor",
                schema: "dbo",
                columns: table => new
                {
                    EventSponsorId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    LargeImage = table.Column<string>(maxLength: 100, nullable: true),
                    Order = table.Column<int>(nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SponsorTitle = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSponsor", x => x.EventSponsorId);
                    table.ForeignKey(
                        name: "FK__EventSpon__Event__12FDD1B2",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSponsor_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventSponsorship",
                schema: "dbo",
                columns: table => new
                {
                    EventSponsorshipId = table.Column<Guid>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Advantage = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    FromAmount = table.Column<decimal>(type: "money", nullable: true),
                    ValAvailable = table.Column<int>(nullable: true),
                    ValSold = table.Column<decimal>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    SumSold = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSponsorship", x => x.EventSponsorshipId);
                    table.ForeignKey(
                        name: "FK_EventSponsorship_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSponsorship_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventTicket",
                schema: "dbo",
                columns: table => new
                {
                    EvenTicketId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonReceiptable = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MaxSpots = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    RegistrationsPerTicket = table.Column<int>(nullable: true),
                    SumRegistrationsAvailable = table.Column<int>(nullable: true),
                    SumRegistrationSold = table.Column<int>(nullable: true),
                    SumAvailable = table.Column<int>(nullable: true),
                    SumSold = table.Column<int>(nullable: true),
                    Tickets = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    ValTickets = table.Column<decimal>(type: "money", nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTicket", x => x.EvenTicketId);
                    table.ForeignKey(
                        name: "FK__EventTick__Event__0697FACD",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTicket_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptLog",
                schema: "dbo",
                columns: table => new
                {
                    ReceiptLogId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    ReceiptStackId = table.Column<Guid>(nullable: true),
                    EntryBy = table.Column<string>(maxLength: 100, nullable: true),
                    EntryReason = table.Column<string>(maxLength: 100, nullable: true),
                    ReceiptNumber = table.Column<string>(maxLength: 100, nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptLog", x => x.ReceiptLogId);
                    table.ForeignKey(
                        name: "FK__ReceiptLo__Recei__7A3223E8",
                        column: x => x.ReceiptStackId,
                        principalSchema: "dbo",
                        principalTable: "ReceiptStack",
                        principalColumn: "ReceiptStackId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSchedule",
                schema: "dbo",
                columns: table => new
                {
                    PaymentScheduleId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    PaymentMethodId = table.Column<Guid>(nullable: true),
                    AppealId = table.Column<Guid>(nullable: true),
                    OriginatingCampaignId = table.Column<Guid>(nullable: true),
                    ConfigurationId = table.Column<Guid>(nullable: true),
                    ConstituentId = table.Column<Guid>(nullable: true),
                    DesignationId = table.Column<Guid>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    EventPackageId = table.Column<Guid>(nullable: true),
                    GiftBatchId = table.Column<Guid>(nullable: true),
                    MembershipCategoryId = table.Column<Guid>(nullable: true),
                    MembershipId = table.Column<Guid>(nullable: true),
                    PackageId = table.Column<Guid>(nullable: true),
                    TaxReceiptId = table.Column<Guid>(nullable: true),
                    TributeId = table.Column<Guid>(nullable: true),
                    TransactionBatchId = table.Column<Guid>(nullable: true),
                    PaymentProcessorId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountMembership = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonReceiptable = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    RecurringAmount = table.Column<decimal>(type: "money", nullable: true),
                    FirstPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FrequencyInterval = table.Column<int>(nullable: true),
                    FrequencyStartCode = table.Column<int>(nullable: true),
                    NextPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Frequency = table.Column<int>(nullable: true),
                    CancelationCode = table.Column<int>(nullable: true),
                    CancellationNote = table.Column<string>(nullable: true),
                    CancelledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndonDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduleTypeCode = table.Column<int>(nullable: true),
                    Anonymity = table.Column<int>(nullable: true),
                    Appraiser = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCity = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCountry = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine3 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingPostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    BillingStateorProvince = table.Column<string>(maxLength: 100, nullable: true),
                    CcBrandCode = table.Column<int>(nullable: true),
                    ChargeonCreate = table.Column<bool>(nullable: true),
                    BookDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GaDeliveryCode = table.Column<int>(nullable: true),
                    DepositDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailAddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionDescription = table.Column<string>(maxLength: 100, nullable: true),
                    DataEntrySource = table.Column<int>(nullable: true),
                    PaymentTypeCode = table.Column<int>(nullable: true),
                    MobilePhone = table.Column<string>(maxLength: 100, nullable: true),
                    OrganizationName = table.Column<string>(maxLength: 100, nullable: true),
                    ReceiptPreferenceCode = table.Column<int>(nullable: true),
                    Telephone1 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone2 = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    DataEntryReference = table.Column<string>(maxLength: 100, nullable: true),
                    MissionInvoiceIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionFraudCode = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionResult = table.Column<string>(maxLength: 100, nullable: true),
                    TributeCode = table.Column<int>(nullable: true),
                    TributeAcknowledgement = table.Column<string>(maxLength: 100, nullable: true),
                    TributeMessage = table.Column<string>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    NumberOfFailures = table.Column<int>(nullable: true),
                    NumberOfSuccesses = table.Column<int>(nullable: true),
                    ConcurrentFailures = table.Column<int>(nullable: true),
                    AmountOfFailures = table.Column<decimal>(type: "money", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: true),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSchedule", x => x.PaymentScheduleId);
                    table.ForeignKey(
                        name: "FK__PaymentSc__Confi__73852659",
                        column: x => x.ConfigurationId,
                        principalSchema: "dbo",
                        principalTable: "Configuration",
                        principalColumn: "ConfigurationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__PaymentSc__Event__756D6ECB",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__PaymentSc__Event__76619304",
                        column: x => x.EventPackageId,
                        principalSchema: "dbo",
                        principalTable: "EventPackage",
                        principalColumn: "EventPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__PaymentSc_To_MembershipCategory",
                        column: x => x.MembershipCategoryId,
                        principalSchema: "dbo",
                        principalTable: "MembershipCategory",
                        principalColumn: "MembershipCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentSchedule_Membership_MembershipId",
                        column: x => x.MembershipId,
                        principalSchema: "dbo",
                        principalTable: "Membership",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__PaymentSc__Payme__719CDDE7",
                        column: x => x.PaymentMethodId,
                        principalSchema: "dbo",
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentSchedule_PaymentProcessor_PaymentProcessorId",
                        column: x => x.PaymentProcessorId,
                        principalSchema: "dbo",
                        principalTable: "PaymentProcessor",
                        principalColumn: "PaymentProcessorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentSchedule_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    EventPackageId = table.Column<Guid>(nullable: true),
                    EventProductId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonreceiptable = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK__Product__EventId__078C1F06",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Product__EventPa__0880433F",
                        column: x => x.EventPackageId,
                        principalSchema: "dbo",
                        principalTable: "EventPackage",
                        principalColumn: "EventPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                schema: "dbo",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    EventPackageId = table.Column<Guid>(nullable: true),
                    EventTicketId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonreceiptable = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    GroupNotes = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    RegistrationsPerTicket = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK__Ticket__EventId__0A688BB1",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Ticket__EventPac__0B5CAFEA",
                        column: x => x.EventPackageId,
                        principalSchema: "dbo",
                        principalTable: "EventPackage",
                        principalColumn: "EventPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sponsorship",
                schema: "dbo",
                columns: table => new
                {
                    SponsorshipId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    EventPackageId = table.Column<Guid>(nullable: true),
                    EventSponsorId = table.Column<Guid>(nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonreceiptable = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsorship", x => x.SponsorshipId);
                    table.ForeignKey(
                        name: "FK__Sponsorsh__Event__15DA3E5D",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Sponsorsh__Event__16CE6296",
                        column: x => x.EventPackageId,
                        principalSchema: "dbo",
                        principalTable: "EventPackage",
                        principalColumn: "EventPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Sponsorsh__Event__17C286CF",
                        column: x => x.EventSponsorId,
                        principalSchema: "dbo",
                        principalTable: "EventSponsor",
                        principalColumn: "EventSponsorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Receipt",
                schema: "dbo",
                columns: table => new
                {
                    ReceiptId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    ReceiptStackId = table.Column<Guid>(nullable: true),
                    PaymentScheduleId = table.Column<Guid>(nullable: true),
                    ReplacesReceiptId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    ExpectedTaxCredit = table.Column<decimal>(type: "money", nullable: true),
                    GeneratedorPrinted = table.Column<double>(nullable: true),
                    LastDonationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmountNonReceiptable = table.Column<decimal>(type: "money", nullable: true),
                    TransactionCount = table.Column<int>(nullable: true),
                    PreferredLanguageCode = table.Column<int>(nullable: true),
                    ReceiptNumber = table.Column<string>(maxLength: 100, nullable: true),
                    ReceiptGeneration = table.Column<int>(nullable: true),
                    ReceiptIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceiptStatus = table.Column<string>(maxLength: 100, nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    Printed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryCode = table.Column<int>(nullable: true),
                    EmailDeliveryStatusCode = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipt", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK__Receipt__Payment__7849DB76",
                        column: x => x.PaymentScheduleId,
                        principalSchema: "dbo",
                        principalTable: "PaymentSchedule",
                        principalColumn: "PaymentScheduleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Receipt__Receipt__7755B73D",
                        column: x => x.ReceiptStackId,
                        principalSchema: "dbo",
                        principalTable: "ReceiptStack",
                        principalColumn: "ReceiptStackId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Receipt__Replace__793DFFAF",
                        column: x => x.ReplacesReceiptId,
                        principalSchema: "dbo",
                        principalTable: "Receipt",
                        principalColumn: "ReceiptId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receipt_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                schema: "dbo",
                columns: table => new
                {
                    RegistrationId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    EventPackageId = table.Column<Guid>(nullable: true),
                    TicketId = table.Column<Guid>(nullable: true),
                    EventTicketId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    GroupNotes = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    Emailaddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone1 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCity = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCountry = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine3 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingPostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    BillingStateorProvince = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 150, nullable: true),
                    Telephone = table.Column<string>(maxLength: 100, nullable: true),
                    Address_Line1 = table.Column<string>(maxLength: 100, nullable: true),
                    Address_Line2 = table.Column<string>(maxLength: 100, nullable: true),
                    Address_City = table.Column<string>(maxLength: 100, nullable: true),
                    Address_Province = table.Column<string>(maxLength: 20, nullable: true),
                    Address_PostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    Address_Country = table.Column<string>(maxLength: 100, nullable: true),
                    Team = table.Column<string>(maxLength: 150, nullable: true),
                    TableId = table.Column<Guid>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.RegistrationId);
                    table.ForeignKey(
                        name: "FK__Registrat__Event__18B6AB08",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Registrat__Event__19AACF41",
                        column: x => x.EventPackageId,
                        principalSchema: "dbo",
                        principalTable: "EventPackage",
                        principalColumn: "EventPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Registrat__Event__1B9317B3",
                        column: x => x.EventTicketId,
                        principalSchema: "dbo",
                        principalTable: "EventTicket",
                        principalColumn: "EvenTicketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Registrat__Ticke__1A9EF37A",
                        column: x => x.TicketId,
                        principalSchema: "dbo",
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registration_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "dbo",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    DesignationId = table.Column<Guid>(nullable: true),
                    OriginatingCampaignId = table.Column<Guid>(nullable: true),
                    ConstituentId = table.Column<Guid>(nullable: true),
                    AppealId = table.Column<Guid>(nullable: true),
                    EventId = table.Column<Guid>(nullable: true),
                    EventPackageId = table.Column<Guid>(nullable: true),
                    GaReturnId = table.Column<Guid>(nullable: true),
                    GiftBatchId = table.Column<Guid>(nullable: true),
                    MembershipId = table.Column<Guid>(nullable: true),
                    MembershipInstanceId = table.Column<Guid>(nullable: true),
                    PackageId = table.Column<Guid>(nullable: true),
                    TaxReceiptId = table.Column<Guid>(nullable: true),
                    DonorCommitmentId = table.Column<Guid>(nullable: true),
                    TransactionBatchId = table.Column<Guid>(nullable: true),
                    TributeId = table.Column<Guid>(nullable: true),
                    ConfigurationId = table.Column<Guid>(nullable: true),
                    TransactionPaymentScheduleId = table.Column<Guid>(nullable: true),
                    TransactionPaymentMethodId = table.Column<Guid>(nullable: true),
                    PaymentProcessorId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    OwningBusinessUnitId = table.Column<Guid>(nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountMembership = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonReceiptable = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountMembership = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountNonreceiptable = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountTax = table.Column<decimal>(type: "money", nullable: true),
                    RefAmount = table.Column<decimal>(type: "money", nullable: true),
                    AmountTransfer = table.Column<decimal>(type: "money", nullable: true),
                    GaAmountClaimed = table.Column<decimal>(type: "money", nullable: true),
                    Anonymity = table.Column<int>(nullable: true),
                    Appraiser = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCity = table.Column<string>(maxLength: 100, nullable: true),
                    BillingCountry = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingLine3 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingPostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    BillingStateorProvince = table.Column<string>(maxLength: 100, nullable: true),
                    CcBrandCode = table.Column<int>(nullable: true),
                    ChargeonCreate = table.Column<bool>(nullable: true),
                    ChequeNumber = table.Column<string>(maxLength: 100, nullable: true),
                    ChequeWireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentRetry = table.Column<int>(nullable: true),
                    BookDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRefunded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GaDeliveryCode = table.Column<int>(nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Emailaddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    GaApplicableCode = table.Column<int>(nullable: true),
                    TransactionDescription = table.Column<string>(maxLength: 100, nullable: true),
                    DataEntrySource = table.Column<int>(nullable: true),
                    PaymentTypeCode = table.Column<int>(nullable: true),
                    LastFailedRetry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    MobilePhone = table.Column<string>(maxLength: 100, nullable: true),
                    NextFailedRetry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationName = table.Column<string>(maxLength: 100, nullable: true),
                    ReceiptPreferenceCode = table.Column<int>(nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Telephone1 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone2 = table.Column<string>(maxLength: 100, nullable: true),
                    ThirdPartyReceipt = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    DataEntryReference = table.Column<string>(maxLength: 100, nullable: true),
                    MissionInvoiceIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionFraudCode = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionNumber = table.Column<string>(maxLength: 255, nullable: true),
                    TransactionResult = table.Column<string>(maxLength: 100, nullable: true),
                    TributeName = table.Column<string>(nullable: true),
                    TributeCode = table.Column<int>(nullable: true),
                    TributeAcknowledgement = table.Column<string>(maxLength: 100, nullable: true),
                    TributeMessage = table.Column<string>(nullable: true),
                    ValidationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidationPerformed = table.Column<bool>(nullable: true),
                    TypeCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    StatusReason = table.Column<int>(nullable: true),
                    DepositDate = table.Column<DateTime>(nullable: true),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployerMatches = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK__Transacti__Confi__625A9A57",
                        column: x => x.ConfigurationId,
                        principalSchema: "dbo",
                        principalTable: "Configuration",
                        principalColumn: "ConfigurationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Transacti__Event__65370702",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Transacti__Membe__625A9A58",
                        column: x => x.MembershipId,
                        principalSchema: "dbo",
                        principalTable: "MembershipCategory",
                        principalColumn: "MembershipCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Transacti__Membe__625A9A59",
                        column: x => x.MembershipInstanceId,
                        principalSchema: "dbo",
                        principalTable: "Membership",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_PaymentProcessor_PaymentProcessorId",
                        column: x => x.PaymentProcessorId,
                        principalSchema: "dbo",
                        principalTable: "PaymentProcessor",
                        principalColumn: "PaymentProcessorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Receipt_TaxReceiptId",
                        column: x => x.TaxReceiptId,
                        principalSchema: "dbo",
                        principalTable: "Receipt",
                        principalColumn: "ReceiptId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Transacti__trans__6166761E",
                        column: x => x.TransactionPaymentMethodId,
                        principalSchema: "dbo",
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Transacti__trans__634EBE90",
                        column: x => x.TransactionPaymentScheduleId,
                        principalSchema: "dbo",
                        principalTable: "PaymentSchedule",
                        principalColumn: "PaymentScheduleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                schema: "dbo",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address1_AddressId = table.Column<Guid>(nullable: true),
                    Address2_AddressId = table.Column<Guid>(nullable: true),
                    MasterId = table.Column<Guid>(nullable: true),
                    OwningBusinessUnitId = table.Column<Guid>(nullable: true),
                    msnfp_GivingLevelId = table.Column<Guid>(nullable: true),
                    msnfp_LastEventPackageId = table.Column<Guid>(nullable: true),
                    msnfp_LastTransactionId = table.Column<Guid>(nullable: true),
                    msnfp_PrimaryMembershipId = table.Column<Guid>(nullable: true),
                    ParentAccountId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Address1_AddressTypeCode = table.Column<int>(nullable: true),
                    Address1_City = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Country = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_County = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Latitude = table.Column<float>(nullable: true),
                    Address1_Line1 = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Line2 = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Line3 = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Longitude = table.Column<float>(nullable: true),
                    Address1_Name = table.Column<string>(maxLength: 150, nullable: true),
                    Address1_PostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_PostOfficeBox = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_StateOrProvince = table.Column<string>(nullable: true),
                    Address2_AddressTypeCode = table.Column<int>(nullable: true),
                    Address2_City = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Country = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_County = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Latitude = table.Column<float>(nullable: true),
                    Address2_Line1 = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Line2 = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Line3 = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Longitude = table.Column<float>(nullable: true),
                    Address2_Name = table.Column<string>(maxLength: 150, nullable: true),
                    Address2_PostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_PostOfficeBox = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_StateOrProvince = table.Column<string>(nullable: true),
                    DoNotBulkEMail = table.Column<bool>(nullable: true),
                    DoNotBulkPostalMail = table.Column<bool>(nullable: true),
                    DoNotEmail = table.Column<bool>(nullable: true),
                    DoNotFax = table.Column<bool>(nullable: true),
                    DoNotPhone = table.Column<bool>(nullable: true),
                    DoNotPostalMail = table.Column<bool>(nullable: true),
                    DoNotSendMM = table.Column<bool>(nullable: true),
                    EmailAddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    EmailAddress2 = table.Column<string>(maxLength: 100, nullable: true),
                    EmailAddress3 = table.Column<string>(maxLength: 100, nullable: true),
                    msnfp_2017ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2017TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2017TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2018ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2018TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2018TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2019ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2019TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2019TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2020ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2020TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2020TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2021ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2021TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2021TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_Anonymous = table.Column<bool>(nullable: true),
                    msnfp_Count_LifetimeTransactions = table.Column<int>(nullable: true),
                    msnfp_LastEventPackageDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    msnfp_LastTransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    msnfp_PreferredLanguageCode = table.Column<int>(nullable: true),
                    msnfp_ReceiptPreferenceCode = table.Column<int>(nullable: true),
                    msnfp_Sum_LifetimeTransactions = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_Telephone1Type = table.Column<int>(nullable: true),
                    msnfp_Telephone2Type = table.Column<int>(nullable: true),
                    msnfp_Telephone3Type = table.Column<int>(nullable: true),
                    msnfp_Vip = table.Column<bool>(nullable: true),
                    Merged = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone1 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone2 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone3 = table.Column<string>(maxLength: 100, nullable: true),
                    WebSiteURL = table.Column<string>(maxLength: 250, nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK__Account__ParentAccountId__0A688BB5",
                        column: x => x.ParentAccountId,
                        principalSchema: "dbo",
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Account__LastEventPackageId__0A688BB2",
                        column: x => x.msnfp_LastEventPackageId,
                        principalSchema: "dbo",
                        principalTable: "EventPackage",
                        principalColumn: "EventPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Account__LastTransactionId__0A688BB3",
                        column: x => x.msnfp_LastTransactionId,
                        principalSchema: "dbo",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Account__PrimaryMembershipId__0A688BB4",
                        column: x => x.msnfp_PrimaryMembershipId,
                        principalSchema: "dbo",
                        principalTable: "Membership",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                schema: "dbo",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address1_AddressId = table.Column<Guid>(nullable: true),
                    Address2_AddressId = table.Column<Guid>(nullable: true),
                    Address3_AddressId = table.Column<Guid>(nullable: true),
                    MasterId = table.Column<Guid>(nullable: true),
                    OwningBusinessUnitId = table.Column<Guid>(nullable: true),
                    msnfp_GivingLevelId = table.Column<Guid>(nullable: true),
                    msnfp_LastEventPackageId = table.Column<Guid>(nullable: true),
                    msnfp_LastTransactionId = table.Column<Guid>(nullable: true),
                    msnfp_PrimaryMembershipId = table.Column<Guid>(nullable: true),
                    ParentCustomerId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    Address1_AddressTypeCode = table.Column<int>(nullable: true),
                    Address1_City = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Country = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_County = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Latitude = table.Column<float>(nullable: true),
                    Address1_Line1 = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Line2 = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Line3 = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_Longitude = table.Column<float>(nullable: true),
                    Address1_Name = table.Column<string>(maxLength: 150, nullable: true),
                    Address1_PostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_PostOfficeBox = table.Column<string>(maxLength: 100, nullable: true),
                    Address1_StateOrProvince = table.Column<string>(nullable: true),
                    Address2_AddressTypeCode = table.Column<int>(nullable: true),
                    Address2_City = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Country = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_County = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Latitude = table.Column<float>(nullable: true),
                    Address2_Line1 = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Line2 = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Line3 = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_Longitude = table.Column<float>(nullable: true),
                    Address2_Name = table.Column<string>(maxLength: 150, nullable: true),
                    Address2_PostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_PostOfficeBox = table.Column<string>(maxLength: 100, nullable: true),
                    Address2_StateOrProvince = table.Column<string>(nullable: true),
                    Address3_AddressTypeCode = table.Column<int>(nullable: true),
                    Address3_City = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_Country = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_County = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_Latitude = table.Column<float>(nullable: true),
                    Address3_Line1 = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_Line2 = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_Line3 = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_Longitude = table.Column<float>(nullable: true),
                    Address3_Name = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_PostalCode = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_PostOfficeBox = table.Column<string>(maxLength: 100, nullable: true),
                    Address3_StateOrProvince = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoNotBulkEMail = table.Column<bool>(nullable: true),
                    DoNotBulkPostalMail = table.Column<bool>(nullable: true),
                    DoNotEmail = table.Column<bool>(nullable: true),
                    DoNotFax = table.Column<bool>(nullable: true),
                    DoNotPhone = table.Column<bool>(nullable: true),
                    DoNotPostalMail = table.Column<bool>(nullable: true),
                    DoNotSendMM = table.Column<bool>(nullable: true),
                    EmailAddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    EmailAddress2 = table.Column<string>(maxLength: 100, nullable: true),
                    EmailAddress3 = table.Column<string>(maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    GenderCode = table.Column<int>(nullable: true),
                    JobTitle = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    msnfp_2017ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2017TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2017TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2018ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2018TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2018TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2019ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2019TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2019TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2020ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2020TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2020TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2021ClassificationCode = table.Column<int>(nullable: true),
                    msnfp_2021TransactionsReceipted = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_2021TransactionsTotal = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_Age = table.Column<int>(nullable: true),
                    msnfp_Anonymous = table.Column<bool>(nullable: true),
                    msnfp_Count_LifetimeTransactions = table.Column<int>(nullable: true),
                    msnfp_LastEventPackageDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    msnfp_LastTransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    msnfp_PreferredLanguageCode = table.Column<int>(nullable: true),
                    msnfp_ReceiptPreferenceCode = table.Column<int>(nullable: true),
                    msnfp_Sum_LifetimeTransactions = table.Column<decimal>(type: "money", nullable: true),
                    msnfp_Telephone1Type = table.Column<int>(nullable: true),
                    msnfp_Telephone2Type = table.Column<int>(nullable: true),
                    msnfp_Telephone3Type = table.Column<int>(nullable: true),
                    msnfp_UpcomingBirthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    msnfp_Vip = table.Column<bool>(nullable: true),
                    Merged = table.Column<bool>(nullable: true),
                    MiddleName = table.Column<string>(maxLength: 100, nullable: true),
                    MobilePhone = table.Column<string>(maxLength: 100, nullable: true),
                    ParentCustomerIdType = table.Column<int>(nullable: true),
                    Salutation = table.Column<string>(maxLength: 100, nullable: true),
                    Suffix = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone1 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone2 = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone3 = table.Column<string>(maxLength: 100, nullable: true),
                    StateCode = table.Column<int>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contact_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Contact__LastEventPackageId__0A688BB6",
                        column: x => x.msnfp_LastEventPackageId,
                        principalSchema: "dbo",
                        principalTable: "EventPackage",
                        principalColumn: "EventPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Contact__LastTransactionId__0A688BB7",
                        column: x => x.msnfp_LastTransactionId,
                        principalSchema: "dbo",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Contact__PrimaryMembershipId__0A688BB8",
                        column: x => x.msnfp_PrimaryMembershipId,
                        principalSchema: "dbo",
                        principalTable: "Membership",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refund",
                schema: "dbo",
                columns: table => new
                {
                    RefundId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerIdType = table.Column<int>(nullable: true),
                    TransactionId = table.Column<Guid>(nullable: true),
                    TransactionCurrencyId = table.Column<Guid>(nullable: true),
                    AmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountMembership = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountMembership = table.Column<decimal>(type: "money", nullable: true),
                    AmountNonReceiptable = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountNonreceiptable = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountReceipted = table.Column<decimal>(type: "money", nullable: true),
                    AmountTax = table.Column<decimal>(type: "money", nullable: true),
                    RefAmountTax = table.Column<decimal>(type: "money", nullable: true),
                    ChequeNumber = table.Column<string>(maxLength: 100, nullable: true),
                    BookDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundTypeCode = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: true),
                    RefAmount = table.Column<decimal>(type: "money", nullable: true),
                    TransactionIdentifier = table.Column<string>(maxLength: 100, nullable: true),
                    TransactionResult = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refund", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_Refund_TransactionCurrency_TransactionCurrencyId",
                        column: x => x.TransactionCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "TransactionCurrency",
                        principalColumn: "TransactionCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Refund__Transact__7C1A6C5A",
                        column: x => x.TransactionId,
                        principalSchema: "dbo",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Response",
                schema: "dbo",
                columns: table => new
                {
                    ResponseId = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    TransactionId = table.Column<Guid>(nullable: true),
                    PaymentScheduleId = table.Column<Guid>(nullable: true),
                    RegistrationPackageId = table.Column<Guid>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Response", x => x.ResponseId);
                    table.ForeignKey(
                        name: "FK__Response__Paymen__7EF6D905",
                        column: x => x.PaymentScheduleId,
                        principalSchema: "dbo",
                        principalTable: "PaymentSchedule",
                        principalColumn: "PaymentScheduleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Response__Regist__7FEAFD3E",
                        column: x => x.RegistrationPackageId,
                        principalSchema: "dbo",
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Response__Transa__7E02B4CC",
                        column: x => x.TransactionId,
                        principalSchema: "dbo",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SyncException",
                schema: "dbo",
                columns: table => new
                {
                    SyncExceptionId = table.Column<Guid>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    PaymentEntityPK = table.Column<Guid>(nullable: true),
                    EntityType = table.Column<string>(nullable: true),
                    ExceptionMessage = table.Column<string>(nullable: true),
                    ExecutionId = table.Column<string>(nullable: true),
                    TransactionId = table.Column<Guid>(nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StateCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncException", x => x.SyncExceptionId);
                    table.ForeignKey(
                        name: "FK_SyncException_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "dbo",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_ParentAccountId",
                schema: "dbo",
                table: "Account",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_TransactionCurrencyId",
                schema: "dbo",
                table: "Account",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_msnfp_LastEventPackageId",
                schema: "dbo",
                table: "Account",
                column: "msnfp_LastEventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_msnfp_LastTransactionId",
                schema: "dbo",
                table: "Account",
                column: "msnfp_LastTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_msnfp_PrimaryMembershipId",
                schema: "dbo",
                table: "Account",
                column: "msnfp_PrimaryMembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_PaymentProcessorId",
                schema: "dbo",
                table: "Configuration",
                column: "PaymentProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_TransactionCurrencyId",
                schema: "dbo",
                table: "Contact",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_msnfp_LastEventPackageId",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_LastEventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_msnfp_LastTransactionId",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_LastTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_msnfp_PrimaryMembershipId",
                schema: "dbo",
                table: "Contact",
                column: "msnfp_PrimaryMembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ConfigurationId",
                schema: "dbo",
                table: "Event",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_DesignationId",
                schema: "dbo",
                table: "Event",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_PaymentProcessorId",
                schema: "dbo",
                table: "Event",
                column: "PaymentProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_TermsOfReferenceId",
                schema: "dbo",
                table: "Event",
                column: "TermsOfReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_TransactionCurrencyId",
                schema: "dbo",
                table: "Event",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventDisclaimer_EventId",
                schema: "dbo",
                table: "EventDisclaimer",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventDonation_EventId",
                schema: "dbo",
                table: "EventDonation",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventDonation_TransactionCurrencyId",
                schema: "dbo",
                table: "EventDonation",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPackage_ConfigurationId",
                schema: "dbo",
                table: "EventPackage",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPackage_EventId",
                schema: "dbo",
                table: "EventPackage",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPackage_PaymentmethodId",
                schema: "dbo",
                table: "EventPackage",
                column: "PaymentmethodId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPackage_TransactionCurrencyId",
                schema: "dbo",
                table: "EventPackage",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPreference_preferencecategoryid",
                schema: "dbo",
                table: "EventPreference",
                column: "preferencecategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_EventProduct_EventId",
                schema: "dbo",
                table: "EventProduct",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventProduct_TransactionCurrencyId",
                schema: "dbo",
                table: "EventProduct",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsor_EventId",
                schema: "dbo",
                table: "EventSponsor",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsor_TransactionCurrencyId",
                schema: "dbo",
                table: "EventSponsor",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsorship_EventId",
                schema: "dbo",
                table: "EventSponsorship",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSponsorship_TransactionCurrencyId",
                schema: "dbo",
                table: "EventSponsorship",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTicket_EventId",
                schema: "dbo",
                table: "EventTicket",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTicket_TransactionCurrencyId",
                schema: "dbo",
                table: "EventTicket",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_MembershipCategoryId",
                schema: "dbo",
                table: "Membership",
                column: "MembershipCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipCategory_TransactionCurrencyId",
                schema: "dbo",
                table: "MembershipCategory",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipOrder_FromMembershipCategoryId",
                schema: "dbo",
                table: "MembershipOrder",
                column: "FromMembershipCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipOrder_ToMembershipGroupId",
                schema: "dbo",
                table: "MembershipOrder",
                column: "ToMembershipGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_RegardingObjectId",
                schema: "dbo",
                table: "Note",
                column: "RegardingObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_PaymentProcessorId",
                schema: "dbo",
                table: "PaymentMethod",
                column: "PaymentProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedule_ConfigurationId",
                schema: "dbo",
                table: "PaymentSchedule",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedule_EventId",
                schema: "dbo",
                table: "PaymentSchedule",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedule_EventPackageId",
                schema: "dbo",
                table: "PaymentSchedule",
                column: "EventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedule_MembershipCategoryId",
                schema: "dbo",
                table: "PaymentSchedule",
                column: "MembershipCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedule_MembershipId",
                schema: "dbo",
                table: "PaymentSchedule",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedule_PaymentMethodId",
                schema: "dbo",
                table: "PaymentSchedule",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedule_PaymentProcessorId",
                schema: "dbo",
                table: "PaymentSchedule",
                column: "PaymentProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedule_TransactionCurrencyId",
                schema: "dbo",
                table: "PaymentSchedule",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Preference_preferencecategoryid",
                schema: "dbo",
                table: "Preference",
                column: "preferencecategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_Product_EventId",
                schema: "dbo",
                table: "Product",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_EventPackageId",
                schema: "dbo",
                table: "Product",
                column: "EventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_TransactionCurrencyId",
                schema: "dbo",
                table: "Product",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_PaymentScheduleId",
                schema: "dbo",
                table: "Receipt",
                column: "PaymentScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_ReceiptStackId",
                schema: "dbo",
                table: "Receipt",
                column: "ReceiptStackId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_ReplacesReceiptId",
                schema: "dbo",
                table: "Receipt",
                column: "ReplacesReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_TransactionCurrencyId",
                schema: "dbo",
                table: "Receipt",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptLog_ReceiptStackId",
                schema: "dbo",
                table: "ReceiptLog",
                column: "ReceiptStackId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptStack_ConfigurationId",
                schema: "dbo",
                table: "ReceiptStack",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_TransactionCurrencyId",
                schema: "dbo",
                table: "Refund",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_TransactionId",
                schema: "dbo",
                table: "Refund",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_EventId",
                schema: "dbo",
                table: "Registration",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_EventPackageId",
                schema: "dbo",
                table: "Registration",
                column: "EventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_EventTicketId",
                schema: "dbo",
                table: "Registration",
                column: "EventTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_TicketId",
                schema: "dbo",
                table: "Registration",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_TransactionCurrencyId",
                schema: "dbo",
                table: "Registration",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Response_PaymentScheduleId",
                schema: "dbo",
                table: "Response",
                column: "PaymentScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Response_RegistrationPackageId",
                schema: "dbo",
                table: "Response",
                column: "RegistrationPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Response_TransactionId",
                schema: "dbo",
                table: "Response",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorship_EventId",
                schema: "dbo",
                table: "Sponsorship",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorship_EventPackageId",
                schema: "dbo",
                table: "Sponsorship",
                column: "EventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorship_EventSponsorId",
                schema: "dbo",
                table: "Sponsorship",
                column: "EventSponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_SyncException_TransactionId",
                schema: "dbo",
                table: "SyncException",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_EventId",
                schema: "dbo",
                table: "Ticket",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_EventPackageId",
                schema: "dbo",
                table: "Ticket",
                column: "EventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TransactionCurrencyId",
                schema: "dbo",
                table: "Ticket",
                column: "TransactionCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ConfigurationId",
                schema: "dbo",
                table: "Transaction",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_EventId",
                schema: "dbo",
                table: "Transaction",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_MembershipId",
                schema: "dbo",
                table: "Transaction",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_MembershipInstanceId",
                schema: "dbo",
                table: "Transaction",
                column: "MembershipInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PaymentProcessorId",
                schema: "dbo",
                table: "Transaction",
                column: "PaymentProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TaxReceiptId",
                schema: "dbo",
                table: "Transaction",
                column: "TaxReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionPaymentMethodId",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionPaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionPaymentScheduleId",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionPaymentScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BankRunSchedule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Contact",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventDisclaimer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventDonation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventPreference",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventProduct",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventSponsorship",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventTable",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GiftAidDeclaration",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MembershipOrder",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Note",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Preference",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReceiptLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReceiptTemplate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Refund",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RegistrationPreference",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RelatedImage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Response",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Sponsorship",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SyncException",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TributeOrMemory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MembershipGroup",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BankRun",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PreferenceCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Registration",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventSponsor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventTicket",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Ticket",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Receipt",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PaymentSchedule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReceiptStack",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventPackage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Membership",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Event",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PaymentMethod",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MembershipCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Configuration",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Designation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TermsOfReference",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TransactionCurrency",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PaymentProcessor",
                schema: "dbo");
        }
    }
}
