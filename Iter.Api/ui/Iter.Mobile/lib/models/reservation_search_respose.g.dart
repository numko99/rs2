// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'reservation_search_respose.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ReservationSearchResponse _$ReservationSearchResponseFromJson(
        Map<String, dynamic> json) =>
    ReservationSearchResponse(
      reservationId: json['reservationId'] as String,
      mainImage: ImageModel.fromJson(json['mainImage'] as Map<String, dynamic>),
      arrangementName: json['arrangementName'] as String,
      arrangementStartDate:
          DateTime.parse(json['arrangementStartDate'] as String),
      arrangementEndDateTime: json['arrangementEndDateTime'] == null
          ? null
          : DateTime.parse(json['arrangementEndDateTime'] as String),
      reservationStatusId: json['reservationStatusId'] as int,
      reservationStatusName: json['reservationStatusName'] as String,
      agencyName: json['agencyName'] as String,
      arrangementPrice: (json['arrangementPrice'] as num).toDouble(),
      totalPaid: (json['totalPaid'] as num).toDouble(),
      reservationDate: DateTime.parse(json['reservationDate'] as String),
      firstName: json['firstName'] as String,
      lastName: json['lastName'] as String,
      reservationNumber: json['reservationNumber'] as String,
      userId: json['userId'] as String?,
    );

Map<String, dynamic> _$ReservationSearchResponseToJson(
        ReservationSearchResponse instance) =>
    <String, dynamic>{
      'reservationId': instance.reservationId,
      'mainImage': instance.mainImage,
      'arrangementName': instance.arrangementName,
      'arrangementStartDate': instance.arrangementStartDate.toIso8601String(),
      'arrangementEndDateTime':
          instance.arrangementEndDateTime?.toIso8601String(),
      'reservationStatusId': instance.reservationStatusId,
      'reservationStatusName': instance.reservationStatusName,
      'agencyName': instance.agencyName,
      'arrangementPrice': instance.arrangementPrice,
      'totalPaid': instance.totalPaid,
      'reservationDate': instance.reservationDate.toIso8601String(),
      'firstName': instance.firstName,
      'lastName': instance.lastName,
      'reservationNumber': instance.reservationNumber,
      'userId': instance.userId,
    };
