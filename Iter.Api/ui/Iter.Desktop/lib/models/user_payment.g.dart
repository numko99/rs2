// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_payment.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserPaymentModel _$UserPaymentModelFromJson(Map<String, dynamic> json) =>
    UserPaymentModel(
      firstName: json['firstName'] as String?,
      lastName: json['lastName'] as String?,
      totalPaid: (json['totalPaid'] as num?)?.toDouble(),
      reservationNumber: json['reservationNumber'] as String?,
      arrangementId: json['arrangementId'] as String?,
      arrangementName: json['arrangementName'] as String?,
    );

Map<String, dynamic> _$UserPaymentModelToJson(UserPaymentModel instance) =>
    <String, dynamic>{
      'firstName': instance.firstName,
      'lastName': instance.lastName,
      'totalPaid': instance.totalPaid,
      'reservationNumber': instance.reservationNumber,
      'arrangementId': instance.arrangementId,
      'arrangementName': instance.arrangementName,
    };
