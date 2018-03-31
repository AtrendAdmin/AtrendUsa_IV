INSERT INTO
MessageTemplate
(Name, [Subject], Body, IsActive, EmailAccountId, LimitedToStores, AttachedDownloadId, DelayPeriodId) 
VALUES
(
'Support.ReturnAuthorizationRequest', 
'%Store.Name% - New RA request:  %Support.FullName% - ra request #%Support.MessageNumber%',
'<p>Brand: %Store.Name%<br />First Name: %SupportModel.FirstName%<br />Last Name: %SupportModel.LastName%<br />Email: %SupportModel.Email%<br />Country: %SupportModel.Country%<br />State / Province / Region: %SupportModel.StateProvince%<br />City: %SupportModel.City%<br />Address Line 1: %SupportModel.AddressLine1%<br />Address Line 2: %SupportModel.AddressLine2%<br />Postal / Zip Code: %SupportModel.PostalCode%<br />Phone Number: %SupportModel.Phone%</p><p>Model #: %SupportModel.ModelNumber%<br />Under Warranty: %ReturnAuthorizationRequestModel.UnderWarranty%<br />Date purchased: %ReturnAuthorizationRequestModel.DatePurchased%<br />Serial #: %SupportModel.SerialNumber%<br />Dealer: %ReturnAuthorizationRequestModel.Dealer%<br />Problem: %SupportModel.Problem%</p>',
1,
1,
0,
0,
0
)