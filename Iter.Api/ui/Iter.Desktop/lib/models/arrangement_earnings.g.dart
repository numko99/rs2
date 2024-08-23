// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'arrangement_earnings.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ArrangementEarnings _$ArrangementEarningsFromJson(Map<String, dynamic> json) =>
    ArrangementEarnings(
      totalPaid: (json['totalPaid'] as num?)?.toDouble(),
      reservationCount: (json['reservationCount'] as num).toInt(),
      arrangementId: json['arrangementId'] as String?,
      arrangementName: json['arrangementName'] as String?,
    );

Map<String, dynamic> _$ArrangementEarningsToJson(
        ArrangementEarnings instance) =>
    <String, dynamic>{
      'totalPaid': instance.totalPaid,
      'reservationCount': instance.reservationCount,
      'arrangementId': instance.arrangementId,
      'arrangementName': instance.arrangementName,
    };
