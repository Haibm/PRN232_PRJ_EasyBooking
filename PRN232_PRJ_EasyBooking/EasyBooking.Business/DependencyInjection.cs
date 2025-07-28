
using EasyBooking.Business.Interfaces;
using EasyBooking.Business.Services;
using EasyBooking.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EasyBooking.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
            services.AddScoped<IShowtimeService, ShowtimeService>();
            services.AddScoped<ICinemaRepository, CinemaRepository>();
            services.AddScoped<ICinemaService, CinemaService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
            services.AddScoped<IOrderHistoryService, OrderHistoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRefundHistoryRepository, RefundHistoryRepository>();
            services.AddScoped<IRefundHistoryService, RefundHistoryService>();
            services.AddScoped<IRefundPolicyRepository, RefundPolicyRepository>();
            services.AddScoped<IRefundPolicyService, RefundPolicyService>();
            
            // Discount System
                    services.AddScoped<IUserPointsRepository, UserPointsRepository>();
        services.AddScoped<IUserPointsService, UserPointsService>();
        services.AddScoped<IPointConfigRepository, PointConfigRepository>();
        services.AddScoped<IPointConfigService, PointConfigService>();
            services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();
            services.AddScoped<IDiscountCodeService, DiscountCodeService>();
            services.AddScoped<IUserDiscountCodeRepository, UserDiscountCodeRepository>();
            services.AddScoped<IUserDiscountCodeService, UserDiscountCodeService>();
            services.AddScoped<IPointTransactionRepository, PointTransactionRepository>();
            services.AddScoped<IPointTransactionService, PointTransactionService>();
            
            return services;
        }
    }
}
