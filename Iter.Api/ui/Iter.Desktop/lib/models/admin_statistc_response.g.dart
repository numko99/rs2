// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'admin_statistc_response.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

AdminStatisticResponse _$AdminStatisticResponseFromJson(
        Map<String, dynamic> json) =>
    AdminStatisticResponse(
      reservationCount: (json['reservationCount'] as num).toInt(),
      arrangementCount: (json['arrangementCount'] as num).toInt(),
      usersCount: (json['usersCount'] as num).toInt(),
      totalAmount: (json['totalAmount'] as num).toDouble(),
      agenciesCount: (json['agenciesCount'] as num).toInt(),
      reservations: (json['reservations'] as List<dynamic>)
          .map((e) =>
              ReservationDiagramResponse.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$AdminStatisticResponseToJson(
        AdminStatisticResponse instance) =>
    <String, dynamic>{
      'reservationCount': instance.reservationCount,
      'arrangementCount': instance.arrangementCount,
      'usersCount': instance.usersCount,
      'totalAmount': instance.totalAmount,
      'agenciesCount': instance.agenciesCount,
      'reservations': instance.reservations,
    };
