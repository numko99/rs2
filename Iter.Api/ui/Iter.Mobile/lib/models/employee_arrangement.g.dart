// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'employee_arrangement.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

EmployeeArrangment _$EmployeeArrangmentFromJson(Map<String, dynamic> json) =>
    EmployeeArrangment(
      id: json['id'] as String?,
      employee: json['employee'] == null
          ? null
          : EmployeeResponse.fromJson(json['employee'] as Map<String, dynamic>),
      arrangement: json['arrangement'] == null
          ? null
          : ArrangementSearchResponse.fromJson(
              json['arrangement'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$EmployeeArrangmentToJson(EmployeeArrangment instance) =>
    <String, dynamic>{
      'id': instance.id,
      'employee': instance.employee,
      'arrangement': instance.arrangement,
    };
