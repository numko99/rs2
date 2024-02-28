// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'reservation.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Reservation _$ReservationFromJson(Map<String, dynamic> json) => Reservation(
      id: json['id'] as String,
      reservationNumber: json['reservationNumber'] as String?,
      arrangementPriceId: json['arrangementPriceId'] as String?,
      departurePlace: json['departurePlace'] as String?,
      user: json['user'] == null
          ? null
          : User.fromJson(json['user'] as Map<String, dynamic>),
      arrangement:
          Arrangement.fromJson(json['arrangement'] as Map<String, dynamic>),
      arrangementPrice: json['arrangementPrice'] == null
          ? null
          : ArrangementPrice.fromJson(
              json['arrangementPrice'] as Map<String, dynamic>),
      reservationStatusId: json['reservationStatusId'] as int,
      reservationStatusName: json['reservationStatusName'] as String,
      totalPaid: (json['totalPaid'] as num).toDouble(),
      rating: json['rating'] as int,
      reminder: json['reminder'] as String,
    );

Map<String, dynamic> _$ReservationToJson(Reservation instance) =>
    <String, dynamic>{
      'id': instance.id,
      'arrangementPriceId': instance.arrangementPriceId,
      'reservationNumber': instance.reservationNumber,
      'departurePlace': instance.departurePlace,
      'user': instance.user,
      'arrangement': instance.arrangement,
      'arrangementPrice': instance.arrangementPrice,
      'reservationStatusId': instance.reservationStatusId,
      'reservationStatusName': instance.reservationStatusName,
      'totalPaid': instance.totalPaid,
      'rating': instance.rating,
      'reminder': instance.reminder,
    };
