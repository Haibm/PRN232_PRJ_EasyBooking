USE [CinemaBookingDB]
GO

/****** Object:  Table [dbo].[RefundPolicy]    Script Date: 7/26/2025 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RefundPolicy](
	[RefundPolicyId] [int] IDENTITY(1,1) NOT NULL,
	[PolicyName] [nvarchar](100) NOT NULL,
	[RefundPercentage] [decimal](5, 2) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[DeleteBy] [nvarchar](50) NULL,
	[DeleteAt] [datetime] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[RefundPolicyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Thêm default values
ALTER TABLE [dbo].[RefundPolicy] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[RefundPolicy] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[RefundPolicy] ADD  DEFAULT ((0)) FOR [IsDelete]
GO

-- Thêm constraint để đảm bảo RefundPercentage trong khoảng 0-100
ALTER TABLE [dbo].[RefundPolicy]  WITH CHECK ADD CHECK  (([RefundPercentage]>=(0) AND [RefundPercentage]<=(100)))
GO

-- Thêm dữ liệu mẫu
INSERT INTO [dbo].[RefundPolicy] ([PolicyName], [RefundPercentage], [Description], [IsActive], [CreateBy], [CreateAt])
VALUES 
('Chính sách hoàn vé mặc định', 80.00, 'Hoàn 80% giá vé gốc cho tất cả trường hợp hoàn vé', 1, 'System', GETDATE()),
('Chính sách hoàn vé VIP', 90.00, 'Hoàn 90% giá vé gốc cho khách hàng VIP', 0, 'System', GETDATE()),
('Chính sách hoàn vé khuyến mãi', 70.00, 'Hoàn 70% giá vé gốc cho các chương trình khuyến mãi', 0, 'System', GETDATE())
GO

-- Thêm index để tối ưu hiệu suất truy vấn
CREATE NONCLUSTERED INDEX [IX_RefundPolicy_IsActive] ON [dbo].[RefundPolicy]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

PRINT 'Bảng RefundPolicy đã được tạo thành công!'
PRINT 'Đã thêm 3 chính sách mẫu:'
PRINT '- Chính sách hoàn vé mặc định (80%) - Đang hoạt động'
PRINT '- Chính sách hoàn vé VIP (90%) - Không hoạt động'
PRINT '- Chính sách hoàn vé khuyến mãi (70%) - Không hoạt động'
GO 