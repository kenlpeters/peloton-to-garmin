﻿using Prometheus;
using System;
using System.Collections.Generic;
using PromMetrics = Prometheus.Metrics;

namespace Common
{
	public static class Metrics
	{
		public static readonly Counter PollsCounter = PromMetrics.CreateCounter("polls_total", "The number of times the current process has polled for new data.");
		public static readonly Histogram WorkoutConversionDuration = PromMetrics.CreateHistogram("workout_conversion_duration_seconds", "Histogram of workout conversion durations.", new HistogramConfiguration() 
		{
			LabelNames = new[] { "format" }
		});


		public static readonly Counter HttpResponseCounter = PromMetrics.CreateCounter("http_responses", "The number of http responses.", new CounterConfiguration
		{
			LabelNames = new[] { "method", "uri", "status_code", "duration_in_seconds" }
		});

		public static readonly Counter HttpErrorCounter = PromMetrics.CreateCounter("http_errors", "The number of errors encountered.", new CounterConfiguration
		{
			LabelNames = new[] { "method", "uri", "status_code", "duration_in_seconds", "message" }
		});

		public static readonly Gauge WorkoutsToConvert = PromMetrics.CreateGauge("workout_conversion_pending", "The number of workouts pending conversion to output format.");

		public static bool ValidateConfig(ObservabilityConfig config)
		{
			if (!config.Prometheus.Enabled)
				return true;

			if (config.Prometheus.Port.HasValue && config.Prometheus.Port <= 0)
			{
				Console.Out.WriteLine($"Prometheus Port must be a valid port: {nameof(config)}.{nameof(config.Prometheus.Port)}.");
				return false;
			}

			return true;
		}
	}
}