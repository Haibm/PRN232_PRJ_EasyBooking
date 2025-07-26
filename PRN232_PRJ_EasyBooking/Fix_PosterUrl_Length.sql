USE [CinemaBookingDB]
GO

-- Tăng độ dài cột PosterUrl từ 255 lên 4000 để chứa được base64 string dài
ALTER TABLE [dbo].[Movies] 
ALTER COLUMN [PosterUrl] [nvarchar](4000) NULL
GO

PRINT 'Đã tăng độ dài cột PosterUrl từ 255 lên 4000 ký tự'
GO 