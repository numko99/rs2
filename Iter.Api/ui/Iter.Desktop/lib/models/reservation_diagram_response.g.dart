// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'reservation_diagram_response.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ReservationDiagramResponse _$ReservationDiagramResponseFromJson(
        Map<String, dynamic> json) =>
    ReservationDiagramResponse(
      date: DateTime.parse(json['date'] as String),
      reservationCount: (json['reservationCount'] as num).toInt(),
    );

Map<String, dynamic> _$ReservationDiagramResponseToJson(
        ReservationDiagramResponse instance) =>
    <String, dynamic>{
      'date': instance.date.toIso8601String(),
      'reservationCount': instance.reservationCount,
    };
