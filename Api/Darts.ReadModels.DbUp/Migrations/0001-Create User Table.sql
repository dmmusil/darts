CREATE TABLE dbo.UserProfiles
(
    UserProfileKey int IDENTITY(1,1) NOT NULL,
    Username nvarchar(100) NOT NULL,
    Email nvarchar(300) NOT NULL
)

CREATE UNIQUE INDEX UIX_Email on dbo.UserProfiles(Email)
CREATE UNIQUE INDEX UIX_Username on dbo.UserProfiles(Username)
ALTER TABLE dbo.UserProfiles ADD CONSTRAINT PK_UserProfiles_UserProfileKey PRIMARY KEY CLUSTERED (UserProfileKey)
