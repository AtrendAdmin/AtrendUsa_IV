INSERT INTO
MessageTemplate
(Name, [Subject], Body, IsActive, EmailAccountId, LimitedToStores, AttachedDownloadId, DelayPeriodId) 
VALUES
(
'Service.NowCommerceSignUp', 
'%NowCommerceSignUp.StoreName%. Now Commerce account sign up request',
'<p>%NowCommerceSignUp.StoreName% <br /><br /> Now Commerce account sign up request from:<br />Dealer Name - %NowCommerceSignUp.DealerName%<br />Contact person - %NowCommerceSignUp.ContactPerson%<br />Contact Email - %NowCommerceSignUp.ContactEmail%<br />Additional Comments - %NowCommerceSignUp.AdditionalComments%</p>',
1,
1,
0,
0,
0
)